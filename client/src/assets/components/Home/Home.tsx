// Home.tsx
import {Fragment, useEffect, useState} from 'react';
import {Link, useNavigate} from 'react-router-dom';
import axios, {AxiosError} from 'axios';
import './Home.css';

interface Task {
    id: string;
    title: string;
    description: string;
    isCompleted: boolean;
    createdAt: string;
    completedAt: string | null;
}

interface Group {
    id: string;
    name: string;
    category: string;
    tasks: Task[];
}

interface ApiResponse<T> {
    statusCode: number;
    isSuccess: boolean;
    data: T;
    error: string | null;
}

export function Home() {
    const navigate = useNavigate();
    const [data, setData] = useState<Group[]>([]);

    // Fetch groups and tasks
    useEffect(() => {
        const token = localStorage.getItem('accessToken');
        if (!token) {
            navigate('/login');
            return;
        }

        const controller = new AbortController();

        // inner async function
        async function fetchData(): Promise<void> {
            try {
                const resp = await axios.get<ApiResponse<Group[]>>(
                    'http://localhost:5000/todo/groups',
                    {headers: {Authorization: `Bearer ${token}`}, signal: controller.signal}
                );
                if (!resp.data.isSuccess) {
                    throw new Error(resp.data.error || 'Fetch failed');
                }
                setData(resp.data.data);
            } catch (err) {
                const ae = err as AxiosError;
                if (ae.response?.status === 401) {
                    localStorage.removeItem('accessToken');
                    navigate('/login');
                } else {
                    console.error('Error fetching data:', err);
                }
            }
        }

        fetchData();
        return () => {
            controller.abort();
        };
    }, [navigate]);

    const handleToggleCompleted = async (groupId: string, taskId: string) => {
        const token = localStorage.getItem('accessToken')!;
        try {
            await axios.patch<ApiResponse<boolean>>(
                'http://localhost:5000/todo/tasks/mark-completed',
                {groupId, taskId},
                {headers: {Authorization: `Bearer ${token}`}}
            );
            setData(prev => prev.map(g => g.id === groupId
                ? {
                    ...g,
                    tasks: g.tasks.map(t => t.id === taskId
                        ? {
                            ...t,
                            isCompleted: !t.isCompleted,
                            completedAt: t.isCompleted ? null : new Date().toISOString()
                        }
                        : t
                    )
                }
                : g
            ));
        } catch (err) {
            console.error('Toggle completed error', err);
        }
    };

    const handleRemoveTask = async (groupId: string, taskId: string) => {
        const token = localStorage.getItem('accessToken')!;
        try {
            await axios.delete(
                `http://localhost:5000/todo/tasks/${taskId}?groupId=${groupId}`,
                {headers: {Authorization: `Bearer ${token}`}}
            );
            setData(prev => prev.map(g =>
                g.id === groupId
                    ? {...g, tasks: g.tasks.filter(t => t.id !== taskId)}
                    : g
            ));
        } catch (err) {
            console.error('Delete task error', err);
        }
    };


    const handleRemoveGroup = async (groupId: string) => {
        const token = localStorage.getItem('accessToken')!;
        try {
            await axios.delete(
                `http://localhost:5000/todo/group/${groupId}`,
                {headers: {Authorization: `Bearer ${token}`}}
            );
            setData(prev => prev.filter(g => g.id !== groupId));
        } catch (err) {
            console.error('Delete group error', err);
        }
    };

    const handleLogout = () => {
        localStorage.removeItem('accessToken');
        navigate('/login');
    };

    return (
        <div className="todo-container">
            <div className="todo-box">
                <h1 className="todo-title">Get Things Done!</h1>
                <table className="todo-table">
                    <thead>
                    <tr>
                        <th>Group Actions</th>
                        <th>Category</th>
                        <th>Name</th>
                        <th>Task Title</th>
                        <th>Description</th>
                        <th>Completed?</th>
                        <th>Created At</th>
                        <th>Completed At</th>
                        <th>Actions</th>
                    </tr>
                    </thead>
                    <tbody>
                    {data.map(group => (
                        <Fragment key={group.id}>
                            <tr className="group-row">
                                <td className="action-cell">
                                    <button
                                        className="btn-action btn-group-rem"
                                        onClick={() => handleRemoveGroup(group.id)}>
                                        Remove Group
                                    </button>
                                </td>
                                <td>{group.category}</td>
                                <td>{group.name}</td>
                                <td colSpan={5} />
                                <td className="action-cell">
                                    <Link
                                        to={`/add-task/${group.id}`}
                                        className="btn-action"
                                    >
                                        Add Task
                                    </Link>
                                </td>
                            </tr>
                            {group.tasks.map(task => (
                                <tr className="task-row" key={task.id}>
                                    <td />
                                    <td />
                                    <td />
                                    <td>{task.title}</td>
                                    <td>{task.description}</td>
                                    <td>
                                        <button
                                            className={task.isCompleted ? 'btn-action btn-uncomplete' : 'btn-action btn-complete'}
                                            onClick={() => handleToggleCompleted(group.id, task.id)}>
                                            {task.isCompleted ? 'Undo' : 'Complete'}
                                        </button>
                                    </td>
                                    <td>{new Date(task.createdAt).toLocaleString()}</td>
                                    <td>{task.completedAt ? new Date(task.completedAt).toLocaleString() : '-'}</td>
                                    <td className="action-cell">
                                        <button
                                            className="btn-action btn-task-rem"
                                            onClick={() => handleRemoveTask(group.id, task.id)}>
                                            Remove Task
                                        </button>
                                    </td>
                                </tr>
                            ))}
                        </Fragment>
                    ))}
                    </tbody>
                </table>
                <div className="footer-actions">
                    <button className="logout-button" onClick={handleLogout}>Logout</button>
                    <Link to="/add-group" className="btn-action btn-add footer-add">Add Group</Link>
                </div>
            </div>
        </div>
    );
}

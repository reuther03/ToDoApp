// Home.tsx
import {Fragment, useEffect, useState} from 'react';
import {Link, useNavigate} from 'react-router-dom';
import axios, {AxiosError} from 'axios';
import './Home.css';

// tworzenie interfejsów dla zadań i grup
// sa to potrzebne do typowania danych
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

// tworzenie komponentu Home
export function Home() {
    // Hooki do nawigacji i stanu
    const navigate = useNavigate();
    const [data, setData] = useState<Group[]>([]);

    // useEffect do pobierania danych po załadowaniu komponentu
    useEffect(() => {
        // sprawdzenie czy użytkownik jest zalogowany
        const token = localStorage.getItem('accessToken');
        if (!token) {
            navigate('/login');
            return;
        }

        // tworzenie kontrolera do anulowania żądania
        const controller = new AbortController();

        // funkcja do pobierania danych z API
        async function fetchData(): Promise<void> {
            try {
                // wysyłanie żądania GET do API
                const resp = await axios.get<ApiResponse<Group[]>>(
                    'http://localhost:5000/todo/groups',
                    {headers: {Authorization: `Bearer ${token}`}, signal: controller.signal}
                );
                // sprawdzenie odpowiedzi i ustawienie danych
                if (!resp.data.isSuccess) {
                    throw new Error(resp.data.error || 'Fetch failed');
                }
                // ustawienie danych w stanie komponentu
                setData(resp.data.data);
            } catch (err) {
                // obsługa błędów
                const ae = err as AxiosError;
                if (ae.response?.status === 401) {
                    // jeśli użytkownik nie jest autoryzowany, usunięcie tokena i przekierowanie do logowania
                    localStorage.removeItem('accessToken');
                    navigate('/login');
                } else {
                    console.error('Error fetching data:', err);
                }
            }
        }

        // wywołanie funkcji pobierającej dane
        fetchData();
        return () => {
            controller.abort();
        };
    }, [navigate]);

    // funkcje do zmiany stanu zadania
    const handleToggleCompleted = async (groupId: string, taskId: string) => {
        // pobranie tokena z localStorage
        const token = localStorage.getItem('accessToken')!;
        try {
            // wysłanie żądania PATCH do API w celu zmiany stanu zadania
            await axios.patch<ApiResponse<boolean>>(
                'http://localhost:5000/todo/tasks/mark-completed',
                {groupId, taskId},
                {headers: {Authorization: `Bearer ${token}`}}
            );
            // aktualizacja stanu komponentu
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
            // logowanie informacji o zakończeniu zmiany stanu
        } catch (err) {
            console.error('Toggle completed error', err);
        }
    };

    // funkcja do usuwania zadania
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


    // funkcja do usuwania grupy
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

    // funkcja do wylogowania użytkownika
    const handleLogout = () => {
        localStorage.removeItem('accessToken');
        navigate('/login');
    };

    // renderowanie komponentu
    return (
        // JSX do renderowania interfejsu użytkownika
        <div className="todo-container">
            <div className="todo-box">
                <h1 className="todo-title">To Do App</h1>
                <table className="todo-table">
                    <thead>
                    <tr>
                        {/* naglowek */}
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
                    {/* pobieranie grup */}
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
                                    {/* dodanie nowego zadania */}
                                    <Link
                                        to={`/add-task/${group.id}`}
                                        className="btn-action"
                                    >
                                        Add Task
                                    </Link>
                                </td>
                            </tr>
                            {/* pobranie zadan */}
                            {group.tasks.map(task => (
                                <tr className="task-row" key={task.id}>
                                    <td />
                                    <td />
                                    <td />
                                    <td>{task.title}</td>
                                    <td>{task.description}</td>
                                    <td>
                                        {/* zmaian stanu zadania */}
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
                {/* dodanie grupy i logout */}
                <div className="footer-actions">
                    <button className="logout-button" onClick={handleLogout}>Logout</button>
                    <Link to="/add-group" className="btn-action btn-add footer-add">Add Group</Link>
                </div>
            </div>
        </div>
    );
}

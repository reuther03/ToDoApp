// AddTask.tsx
import React, { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import axios, { type AxiosError } from 'axios';
import './ToDo.css';

// Definiowanie interfejsu dla odpowiedzi z serwera
export interface AddTaskResponse {
    statusCode: number;
    isSuccess: boolean;
    data: string; // new task GUID
    error: string | null;
}

// Komponent do dodawania zadania
export default function AddTask() {
    const { groupId } = useParams<{ groupId: string }>();
    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();
    const token = localStorage.getItem('accessToken')!;

    // obsługa wysłania formularza dodawania zadania
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);

        if (!groupId) {
            setError('Invalid group identifier.');
            return;
        }

        try {
            const resp = await axios.post<AddTaskResponse>(
                'http://localhost:5000/todo/tasks',
                { groupId, title, description },
                { headers: { Authorization: `Bearer ${token}` } }
            );
            if (!resp.data.isSuccess) throw new Error(resp.data.error || 'Add task failed');
            navigate('/');
        } catch (err: unknown) {
            if (axios.isCancel(err)) return;
            const axErr = err as AxiosError<{ message?: string }>;
            setError(axErr.response?.data.message ?? 'Unable to add task.');
        }
    };

    // renderowanie formularza dodawania zadania
    return (
        <div className="form-container">
            <form className="task-form" onSubmit={handleSubmit}>
                <h2>Add Task</h2>
                {error && <div className="error-msg">{error}</div>}

                <label>
                    Title
                    <input
                        type="text"
                        value={title}
                        onChange={e => setTitle(e.target.value)}
                        required
                    />
                </label>

                <label>
                    Description
                    <textarea
                        value={description}
                        onChange={e => setDescription(e.target.value)}
                        required
                    />
                </label>

                <button type="submit" className="btn-action btn-add-task">
                    Save Task
                </button>
            </form>
        </div>
    );
}

// AddGroup.tsx
import React, {useState} from 'react';
import {useNavigate} from 'react-router-dom';
import axios, {type AxiosError} from 'axios';
import './ToDo.css';

// Definiowanie interfejsu dla odpowiedzi z serwera
export interface AddGroupResponse {
    statusCode: number;
    isSuccess: boolean;
    data: string;
    error: string | null;
}

// Lista kategorii do wyboru
const categories = [
    {label: 'Work', value: 0},
    {label: 'Personal', value: 1},
    {label: 'Shopping', value: 2},
    {label: 'Health', value: 3},
    {label: 'Finance', value: 4},
    {label: 'Education', value: 5},
    {label: 'Travel', value: 6},
    {label: 'Other', value: 7},
];

// Komponent do dodawania grupy
export default function AddGroup() {
    const [title, setTitle] = useState('');
    const [category, setCategory] = useState<number | ''>('');
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();
    const token = localStorage.getItem('accessToken')!;

    // Funkcja obsługująca wysłanie formularza
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);
        if (category === '') {
            setError('Please select a category.');
            return;
        }

        try {
            const resp = await axios.post<AddGroupResponse>(
                'http://localhost:5000/todo/group',
                {title, category},
                {headers: {Authorization: `Bearer ${token}`}}
            );
            if (!resp.data.isSuccess) throw new Error(resp.data.error || 'Add group failed');
            navigate('/');
        } catch (err: unknown) {
            if (axios.isCancel(err)) return;
            const axErr = err as AxiosError<{ message?: string }>;
            setError(axErr.response?.data.message ?? 'Unable to add group.');
        }
    };

    // Renderowanie formularza dodawania grupy
    return (
        <div className="form-container">
            <form className="group-form" onSubmit={handleSubmit}>
                <h2>Add Group</h2>
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
                    Category
                    <select
                        value={category}
                        onChange={e => setCategory(Number(e.target.value))}
                        required
                    >
                        <option value="">-- Select category --</option>
                        {categories.map(c => (
                            <option key={c.value} value={c.value}>
                                {c.label}
                            </option>
                        ))}
                    </select>
                </label>

                <button type="submit" className="btn-action btn-add">
                    Save Group
                </button>
            </form>
        </div>
    );
}

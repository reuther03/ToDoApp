import React, {useState} from 'react';
import axios, {type AxiosError} from 'axios';
import {useNavigate} from 'react-router-dom';
import './Register.css';

// definiowanie interfejsów dla danych rejestracji i odpowiedzi
export interface SignUpResponse {
    statusCode: number;
    isSuccess: boolean;
    data: string;
    error: string | null;
}

export default function Register() {
    // hooki do zarządzania stanem i nawigacją
    const [email, setEmail] = useState('');
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();

    // funkcja obsługująca wysłanie formularza rejestracji
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);

        try {
            const response = await axios.post<SignUpResponse>(
                'http://localhost:5000/user/signup',
                {email, username, password},
                {headers: {'Content-Type': 'application/json'}}
            );

            if (!response.data.isSuccess) {
                throw new Error(response.data.error ?? 'Registration failed');
            }

            setTimeout(() => navigate('/login'), 1500);
        } catch (err: unknown) {
            if (axios.isCancel(err)) return;
            const axErr = err as AxiosError<{ message?: string }>;
            setError(axErr.response?.data.message ?? 'Registration failed');
        }
    };

    // renderowanie formularza rejestracji
    return (
        <div className="register-container">
            <form className="register-form" onSubmit={handleSubmit}>
                <h2>Create Account</h2>
                {error && <div className="error-msg">{error}</div>}

                <label>
                    Email
                    <input
                        type="email"
                        value={email}
                        onChange={e => setEmail(e.target.value)}
                        required
                    />
                </label>
                <label>
                    Username
                    <input
                        type="text"
                        value={username}
                        onChange={e => setUsername(e.target.value)}
                        required
                    />
                </label>
                <label>
                    Password
                    <input
                        type="password"
                        value={password}
                        onChange={e => setPassword(e.target.value)}
                        required
                    />
                </label>

                <button type="submit">Register</button>
            </form>
        </div>
    );
}

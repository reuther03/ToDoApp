import React, {useState} from 'react';
import axios, {type AxiosError} from 'axios';
import {useNavigate} from 'react-router-dom';
import './Login.css';

interface LoginData {
    token: string;
    userId: string;
    username: string;
}

interface LoginResponse {
    statusCode: number;
    isSuccess: boolean;
    data: LoginData;
    error: string | null;
}

export default function Login() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const response = await axios.post<LoginResponse>(
                'http://localhost:5000/user/login',
                {email, password},
                {headers: {'Content-Type': 'application/json'}}
            );

            if (!response.data.isSuccess) {
                throw new Error(response.data.error ?? 'Login failed');
            }

            const {token} = response.data.data;
            localStorage.setItem('accessToken', token);
            navigate('/');
        } catch (err: unknown) {
            if (axios.isCancel(err)) return;
            const axErr = err as AxiosError<{ message?: string }>;
            setError(axErr.response?.data.message ?? 'Login failed');
        }
    };

    return (
        <div className="login-container">
            <form className="login-form" onSubmit={handleSubmit}>
                <h2>Login</h2>
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
                    Password
                    <input
                        type="password"
                        value={password}
                        onChange={e => setPassword(e.target.value)}
                        required
                    />
                </label>
                <button type="submit">Login</button>
            </form>
        </div>
    );
}

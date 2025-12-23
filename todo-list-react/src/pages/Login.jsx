import { useState } from 'react';
import './Login.css'; // Import styles

const Login = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setLoading(true);

        try {
            // Example request to your ASP.NET Core API
            const response = await fetch('/api/v1/auth/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    login: email,
                    password: password
                }),
            });

            if (!response.ok) {
                const data = await response.json();
                throw new Error(data.message || 'Invalid email or password');
            }

            const data = await response.json();
            // Save token (JWT example)
            localStorage.setItem('token', data.token);

            // Redirect to home page or todo list
            window.location.href = '/'; // or use react-router navigation
        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="login-container">
            <div className="login-card">
                <h2>Sign In</h2>
                <p className="subtitle">Enter your credentials to access your account</p>

                {error && <div className="error-message">{error}</div>}

                <form onSubmit={handleSubmit}>
                    <div className="input-group">
                        <label htmlFor="email">Email</label>
                        <input
                            type="email"
                            id="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            required
                            placeholder="example@mail.com"
                            disabled={loading}
                        />
                    </div>

                    <div className="input-group">
                        <label htmlFor="password">Password</label>
                        <input
                            type="password"
                            id="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            required
                            placeholder=""
                            disabled={loading}
                        />
                    </div>

                    <button type="submit" disabled={loading} className="login-btn">
                        {loading ? 'Signing in...' : 'Sign In'}
                    </button>
                </form>

                <div className="footer-links">
                    <a href="/register">Don't have an account? Register</a>
                    <a href="/forgot">Forgot password?</a>
                </div>
            </div>
        </div>
    );
};

export default Login;
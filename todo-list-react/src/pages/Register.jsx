import { useState } from 'react';
import './Login.css'; // Import styles

const Register = () => {
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
            const response = await fetch('/api/v1/auth/register', {
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
                <h1>TODO List</h1>
                <h2>Register</h2>
                <p className="subtitle">Enter your credentials to create an account</p>

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

                    <div>
                        <button type="submit" disabled={loading} className="login-btn">
                            {loading ? 'Registering...' : 'Register'}
                        </button>
                    </div>

                </form>

                <div className="footer-links">
                    <a href="/login">Already have an account? Sign In</a>
                    <a href="/forgot">Forgot password?</a>
                </div>
            </div>
        </div>
    );
};


export default Register;
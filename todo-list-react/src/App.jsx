import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import Login from './pages/Login';        
import TodoList from './pages/TodoList';  
import Register from './pages/Register';  
import NotFound from './pages/NotFound';  


function useAuth() {
    return !!localStorage.getItem('token');
}

function ProtectedRoute({ children }) {
    const isAuthenticated = useAuth();
    return isAuthenticated ? children : <Navigate to="/login" replace />;
}

function PublicRoute({ children }) {
    const isAuthenticated = useAuth();
    return isAuthenticated ? <Navigate to="/" replace /> : children;
}

function TopBar() {
    const isAuthenticated = useAuth();

    function handleLogout() {
        localStorage.removeItem('token');
        window.location.reload();
    }

    if (!isAuthenticated) return null;

    return (
        <header className="top-bar" style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', padding: '8px 16px', borderBottom: '1px solid #e5e5e5' }}>
            <div className="app-title" style={{ fontWeight: 600 }}>Todo List</div>
            <button
                type="button"
                onClick={handleLogout}
                className="logout-button"
                style={{ padding: '6px 12px', cursor: 'pointer' }}
            >
                Вийти
            </button>
        </header>
    );
}

function Footer() {
    const isAuthenticated = useAuth();
    if (!isAuthenticated) return null;

    const year = new Date().getFullYear();
    return (
        <footer className="app-footer" style={{ padding: '12px 16px', borderTop: '1px solid #e5e5e5', textAlign: 'center', fontSize: '14px', color: '#666' }}>
            <div>Todo List — приложение для управления задачами</div>
            <div style={{ marginTop: 4 }}>Версия 1.0.0 • © {year}</div>
        </footer>
    );
}

function App() {
    return (

        <BrowserRouter>
            <div className="App">
                <TopBar />
                <Routes>
                    <Route
                        path="/"
                        element={
                            <ProtectedRoute>
                                <TodoList />
                            </ProtectedRoute>
                        }
                    />

                    <Route
                        path="/login"
                        element={
                            <PublicRoute>
                                <Login />
                            </PublicRoute>
                        }
                    />

                    <Route
                        path="/register"
                        element={
                            <PublicRoute>
                                <Register />
                            </PublicRoute>
                        }
                    />

                    <Route path="*" element={<NotFound />} />
                </Routes>

                <Footer />

            </div>
        </BrowserRouter>
    );
}

export default App;
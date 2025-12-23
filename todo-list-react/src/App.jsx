import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import Login from './pages/Login';          // Страница авторизации, которую мы сделали
import TodoList from './pages/TodoList';    // Ваша основная страница с задачами
import Register from './pages/Register';    // Опционально: регистрация
import NotFound from './pages/NotFound';     // 404 страница

// Простой хук для проверки авторизации (по токену в localStorage)
function useAuth() {
    return !!localStorage.getItem('token');
}

// Защищённый маршрут — только для авторизованных пользователей
function ProtectedRoute({ children }) {
    const isAuthenticated = useAuth();
    return isAuthenticated ? children : <Navigate to="/login" replace />;
}

// Публичный маршрут — перенаправляет авторизованных на главную
function PublicRoute({ children }) {
    const isAuthenticated = useAuth();
    return isAuthenticated ? <Navigate to="/" replace /> : children;
}

function App() {
    return (
        <BrowserRouter>
            <div className="App">
                <Routes>
                    {/* Главная страница — список задач (защищена) */}
                    <Route
                        path="/"
                        element={
                            <ProtectedRoute>
                                <TodoList />
                            </ProtectedRoute>
                        }
                    />

                    {/* Страница входа */}
                    <Route
                        path="/login"
                        element={
                            <PublicRoute>
                                <Login />
                            </PublicRoute>
                        }
                    />

                    {/* Регистрация (опционально) */}
                    <Route
                        path="/register"
                        element={
                            <PublicRoute>
                                <Register />
                            </PublicRoute>
                        }
                    />

                    {/* 404 — страница не найдена */}
                    <Route path="*" element={<NotFound />} />
                </Routes>
            </div>
        </BrowserRouter>
    );
}

export default App;
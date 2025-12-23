import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import Login from './pages/Login';          // �������� �����������, ������� �� �������
import TodoList from './pages/TodoList';    // ���� �������� �������� � ��������
import Register from './pages/Register';    // �����������: �����������
import NotFound from './pages/NotFound';     // 404 ��������


function useAuth() {
    return !!localStorage.getItem('token');
}

// ���������� ������� � ������ ��� �������������� �������������
function ProtectedRoute({ children }) {
    const isAuthenticated = useAuth();
    return isAuthenticated ? children : <Navigate to="/login" replace />;
}

// ��������� ������� � �������������� �������������� �� �������
function PublicRoute({ children }) {
    const isAuthenticated = useAuth();
    return isAuthenticated ? <Navigate to="/" replace /> : children;
}

function App() {
    return (
        <BrowserRouter>
            <div className="App">
                <Routes>
                    {/* ������� �������� � ������ ����� (��������) */}
                    <Route
                        path="/"
                        element={
                            <ProtectedRoute>
                                <TodoList />
                            </ProtectedRoute>
                        }
                    />

                    {/* �������� ����� */}
                    <Route
                        path="/login"
                        element={
                            <PublicRoute>
                                <Login />
                            </PublicRoute>
                        }
                    />

                    {/* ����������� (�����������) */}
                    <Route
                        path="/register"
                        element={
                            <PublicRoute>
                                <Register />
                            </PublicRoute>
                        }
                    />

                    {/* 404 � �������� �� ������� */}
                    <Route path="*" element={<NotFound />} />
                </Routes>
            </div>
        </BrowserRouter>
    );
}

export default App;
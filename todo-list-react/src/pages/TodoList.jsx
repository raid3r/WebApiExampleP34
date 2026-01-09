import React, { useEffect, useState } from 'react';

// Пример данных
const sampleData = [
    //{
    //    id: 2,
    //    name: 'My first todo list',
    //    items: [
    //        {
    //            id: 7,
    //            title: 'Create React frontent project',
    //            isCompleted: false,
    //            description: 'With ASP.NET wabApi backend',
    //            priority: 2,
    //        },
    //    ],
    //},
];

// Стили
const styles = {
    container: {
        fontFamily: '"Segoe UI", Roboto, "Helvetica Neue", Arial',
        padding: '20px',
        maxWidth: 900,
        margin: '0 auto',
        color: '#222',
    },
    listCard: {
        background: '#fff',
        borderRadius: 8,
        boxShadow: '0 1px 3px rgba(0,0,0,0.08)',
        padding: 16,
        marginBottom: 20,
    },
    title: {
        margin: '0 0 12px 0',
        fontSize: 20,
        color: '#111827',
    },
    headerRow: {
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'space-between',
        gap: 12,
    },
    table: {
        width: '100%',
        borderCollapse: 'collapse',
        overflow: 'hidden',
    },
    th: {
        textAlign: 'left',
        padding: '10px 12px',
        background: '#f3f4f6',
        fontSize: 13,
        color: '#374151',
        borderBottom: '1px solid #e5e7eb',
    },
    td: {
        padding: '10px 12px',
        borderBottom: '1px solid #eee',
        fontSize: 14,
        verticalAlign: 'top',
    },
    priorityBadge: {
        display: 'inline-block',
        padding: '4px 8px',
        borderRadius: 999,
        fontSize: 12,
        color: '#fff',
    },
    completed: {
        textDecoration: 'line-through',
        color: '#6b7280',
    },
    empty: {
        textAlign: 'center',
        padding: 30,
        color: '#6b7280',
    },
    // Стили формы
    formRow: {
        display: 'flex',
        gap: 8,
        marginTop: 12,
        alignItems: 'flex-start',
        flexWrap: 'wrap',
    },
    input: {
        padding: '8px 10px',
        borderRadius: 6,
        border: '1px solid #d1d5db',
        fontSize: 14,
        minWidth: 180,
        flex: '1 1 220px',
    },
    textarea: {
        padding: '8px 10px',
        borderRadius: 6,
        border: '1px solid #d1d5db',
        fontSize: 14,
        minWidth: 220,
        flex: '1 1 320px',
        resize: 'vertical',
        minHeight: 60,
    },
    select: {
        padding: '8px 10px',
        borderRadius: 6,
        border: '1px solid #d1d5db',
        fontSize: 14,
        background: '#fff',
    },
    checkboxLabel: {
        display: 'flex',
        alignItems: 'center',
        gap: 6,
        fontSize: 14,
        color: '#374151',
    },
    button: {
        padding: '8px 12px',
        borderRadius: 6,
        border: 'none',
        background: '#3b82f6',
        color: '#fff',
        cursor: 'pointer',
        fontSize: 14,
    },
    buttonDisabled: {
        background: '#93c5fd',
        cursor: 'not-allowed',
    },
    deleteButton: {
        padding: '8px 12px',
        borderRadius: 6,
        border: 'none',
        background: '#ef4444',
        color: '#fff',
        cursor: 'pointer',
        fontSize: 14,
    },
    deleteButtonDisabled: {
        background: '#fca5a5',
        cursor: 'not-allowed',
    },
    smallNote: {
        fontSize: 12,
        color: '#6b7280',
        marginTop: 8,
    },
};

// Метка приоритета
const priorityLabel = (p) => {
    switch (p) {
        case 1:
            return { text: 'Low', color: '#10b981' };
        case 2:
            return { text: 'Medium', color: '#f59e0b' };
        case 3:
            return { text: 'High', color: '#ef4444' };
        default:
            return { text: 'N/A', color: '#6b7280' };
    }
};

const TodoList = () => {
    const [lists, setLists] = useState(sampleData);
    const [loading, setLoading] = useState(false);

    // Состояние форм добавления задач: { [listId]: { title, description, priority, isCompleted, saving } }
    const [forms, setForms] = useState({});

    // Новое состояние для создания списка
    const [newListName, setNewListName] = useState('');
    const [creatingList, setCreatingList] = useState(false);

    // Инициализация пустых форм при загрузке данных
    useEffect(() => {
        if (Array.isArray(lists)) {
            const initial = {};
            lists.forEach((l) => {
                initial[l.id] = {
                    title: '',
                    description: '',
                    priority: 2,
                    isCompleted: false,
                    saving: false,
                };
            });
            setForms((prev) => ({ ...initial, ...prev }));
        }
    }, [lists]);

    // Получение данных
    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            try {
                const response = await fetch('/api/v1/todo-list/list', {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + localStorage.getItem('token'),
                    },
                });
                if (response.ok) {
                    const data = await response.json();
                    // Ожидание, что data — массив списков
                    if (Array.isArray(data) && data.length > 0) {
                        setLists(data);
                    } else {
                        // Не менять sample при пустом ответе
                        setLists((prev) => (prev.length ? prev : []));
                    }
                } else {
                    // На ошибку сервера оставляем примерные данные
                    console.warn('API returned status', response.status);
                }
            } catch (err) {
                console.error(err);
            } finally {
                setLoading(false);
            }
        };
        fetchData();
    }, []);

    const updateFormField = (listId, field, value) => {
        setForms((prev) => ({
            ...prev,
            [listId]: {
                ...(prev[listId] || { title: '', description: '', priority: 2, isCompleted: false }),
                [field]: value,
            },
        }));
    };

    // Проверка временного id
    const isTempId = (id) => typeof id === 'string' && id.startsWith('tmp-');

    // Создание нового списка (оптимистично)
    const handleCreateList = async (e) => {
        e.preventDefault();
        const name = (newListName || '').trim();
        if (!name || creatingList) return;

        setCreatingList(true);
        const tempId = `tmp-list-${Date.now()}-${Math.floor(Math.random() * 10000)}`;
        const tempList = { id: tempId, name, items: [] };

        // Оптимистично добавляем список в UI и инициализируем форму
        setLists((prev) => [...prev, tempList]);
        setForms((prev) => ({
            ...prev,
            [tempId]: {
                title: '',
                description: '',
                priority: 2,
                isCompleted: false,
                saving: false,
            },
            ...prev,
        }));

        try {
            const response = await fetch('/api/v1/todo-list/create', {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + localStorage.getItem('token'),
                },
                body: JSON.stringify({ name }),
            });

            if (response.ok) {
                const result = await response.json();
                const created = result && result.data ? result.data : null;
                if (created && created.id) {
                    // Заменяем временный список на серверный
                    setLists((prev) =>
                        prev.map((l) => (l.id === tempId ? { ...created, items: Array.isArray(created.items) ? created.items : [] } : l))
                    );
                    // Переносим форму с tempId на created.id
                    setForms((prev) => {
                        const copied = { ...prev };
                        if (copied[tempId]) {
                            const tempForm = copied[tempId];
                            delete copied[tempId];
                            copied[created.id] = tempForm;
                        } else {
                            copied[created.id] = {
                                title: '',
                                description: '',
                                priority: 2,
                                isCompleted: false,
                                saving: false,
                            };
                        }
                        return copied;
                    });
                } else {
                    console.warn('Server did not return created list object');
                    // Оставляем временный список, т.к. сервер мог принять, но не вернуть объект
                }
            } else {
                console.warn('Failed to create list, status', response.status);
                // Удаляем временный список
                setLists((prev) => prev.filter((l) => l.id !== tempId));
                setForms((prev) => {
                    const copied = { ...prev };
                    delete copied[tempId];
                    return copied;
                });
            }
        } catch (err) {
            console.error(err);
            // Откат при ошибке
            setLists((prev) => prev.filter((l) => l.id !== tempId));
            setForms((prev) => {
                const copied = { ...prev };
                delete copied[tempId];
                return copied;
            });
        } finally {
            setCreatingList(false);
            setNewListName('');
        }
    };

    // Удаление списка (оптимистично)
    const handleDeleteList = async (listId) => {
        const list = lists.find((l) => l.id === listId);
        if (!list) return;

        // Если временный список — просто удалить локально
        if (isTempId(listId)) {
            setLists((prev) => prev.filter((l) => l.id !== listId));
            setForms((prev) => {
                const copied = { ...prev };
                delete copied[listId];
                return copied;
            });
            return;
        }

        // Пометить список как удаляющийся
        setLists((prev) => prev.map((l) => (l.id === listId ? { ...l, _deleting: true } : l)));

        try {
            const response = await fetch(`/api/v1/todo-list/${listId}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + localStorage.getItem('token'),
                },
            });

            if (response.ok) {
                // Успешно — удалить из списка и форму
                setLists((prev) => prev.filter((l) => l.id !== listId));
                setForms((prev) => {
                    const copied = { ...prev };
                    delete copied[listId];
                    return copied;
                });
            } else {
                console.warn('Failed to delete list, status', response.status);
                // Снять флаг удаления
                setLists((prev) => prev.map((l) => (l.id === listId ? { ...l, _deleting: false } : l)));
            }
        } catch (err) {
            console.error(err);
            // Снять флаг удаления при ошибке
            setLists((prev) => prev.map((l) => (l.id === listId ? { ...l, _deleting: false } : l)));
        }
    };

    // Переключение completed с оптимистичным обновлением и сохранением на сервере
    const handleToggleCompleted = async (listId, itemId) => {
        // Найти текущие значения
        const list = lists.find((l) => l.id === listId);
        if (!list || !Array.isArray(list.items)) return;

        const item = list.items.find((it) => it.id === itemId);
        if (!item) return;

        // Для временных элементов просто переключаем локально
        if (isTempId(itemId)) {
            setLists((prev) =>
                prev.map((l) =>
                    l.id !== listId
                        ? l
                        : {
                              ...l,
                              items: l.items.map((it) => (it.id === itemId ? { ...it, isCompleted: !it.isCompleted } : it)),
                          }
                )
            );
            return;
        }

        const prevValue = !!item.isCompleted;
        const newValue = !prevValue;

        item.isCompleted = newValue

        // Оптимистично помечаем _saving и переключаем значение
        setLists((prev) =>
            prev.map((l) =>
                l.id !== listId
                    ? l
                    : {
                          ...l,
                          items: l.items.map((it) =>
                              it.id === itemId ? { ...it, isCompleted: newValue, _saving: true } : it
                          ),
                      }
            )
        );

        try {
            const response = await fetch(`/api/v1/todo/${itemId}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + localStorage.getItem('token'),
                },
                body: JSON.stringify(item),
            });

            if (response.ok) {
                const result = await response.json();
                const serverItem = result && result.data ? result.data : null;

                setLists((prev) =>
                    prev.map((l) =>
                        l.id !== listId
                            ? l
                            : {
                                  ...l,
                                  items: l.items.map((it) => {
                                      if (it.id !== itemId) return it;
                                      if (serverItem && serverItem.id) {
                                          // Заменяем на серверную версию
                                          return { ...serverItem, _saving: false };
                                      }
                                      // Сервер не вернул объект — убрать _saving и оставить isCompleted
                                      const { _saving, ...rest } = { ...it, _saving: false };
                                      return rest;
                                  }),
                              }
                    )
                );
            } else {
                console.warn('Failed to update item, status', response.status);
                // Откатить значение
                setLists((prev) =>
                    prev.map((l) =>
                        l.id !== listId
                            ? l
                            : {
                                  ...l,
                                  items: l.items.map((it) => (it.id === itemId ? { ...it, isCompleted: prevValue, _saving: false } : it)),
                              }
                    )
                );
            }
        } catch (err) {
            console.error(err);
            // Откатить при ошибке
            setLists((prev) =>
                prev.map((l) =>
                    l.id !== listId
                        ? l
                        : {
                              ...l,
                              items: l.items.map((it) => (it.id === itemId ? { ...it, isCompleted: prevValue, _saving: false } : it)),
                          }
                )
            );
        }
    };

    // Удаление задачи (оптимистично: помечаем _deleting, затем удаляем; для временных id просто удаляем локально)
    const handleDeleteItem = async (listId, itemId) => {
        const list = lists.find((l) => l.id === listId);
        if (!list || !Array.isArray(list.items)) return;

        // Если временный элемент — просто удалить локально
        if (isTempId(itemId)) {
            setLists((prev) =>
                prev.map((l) => (l.id !== listId ? l : { ...l, items: l.items.filter((it) => it.id !== itemId) }))
            );
            return;
        }

        // Помечаем элемент как удаляющийся
        setLists((prev) =>
            prev.map((l) =>
                l.id !== listId
                    ? l
                    : {
                          ...l,
                          items: l.items.map((it) => (it.id === itemId ? { ...it, _deleting: true } : it)),
                      }
            )
        );

        try {
            const response = await fetch(`/api/v1/todo/${itemId}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + localStorage.getItem('token'),
                },
            });

            if (response.ok) {
                // Успешно — удалить из списка
                setLists((prev) =>
                    prev.map((l) => (l.id !== listId ? l : { ...l, items: l.items.filter((it) => it.id !== itemId) }))
                );
            } else {
                console.warn('Failed to delete item, status', response.status);
                // Снять флаг удаления
                setLists((prev) =>
                    prev.map((l) =>
                        l.id !== listId
                            ? l
                            : {
                                  ...l,
                                  items: l.items.map((it) => (it.id === itemId ? { ...it, _deleting: false } : it)),
                              }
                    )
                );
            }
        } catch (err) {
            console.error(err);
            // Снять флаг удаления при ошибке
            setLists((prev) =>
                prev.map((l) =>
                    l.id !== listId
                        ? l
                        : {
                              ...l,
                              items: l.items.map((it) => (it.id === itemId ? { ...it, _deleting: false } : it)),
                          }
                )
            );
        }
    };

    // Добавление задачи (оптимистично)
    const handleAddItem = async (e, listId) => {
        e.preventDefault();
        const form = forms[listId];
        if (!form || !form.title || form.saving) {
            return;
        }

        // Пометить как сохраняющийся
        updateFormField(listId, 'saving', true);

        // Оптимистичная задача
        const tempId = `tmp-${Date.now()}-${Math.floor(Math.random() * 10000)}`;
        const newItem = {
            id: tempId,
            title: form.title,
            description: form.description,
            priority: form.priority,
            isCompleted: !!form.isCompleted,
        };

        // Добавляем оптимистично в UI
        setLists((prev) =>
            prev.map((l) => (l.id === listId ? { ...l, items: Array.isArray(l.items) ? [newItem, ...l.items] : [newItem] } : l))
        );

        try {
            const response = await fetch(`/api/v1/todo-list/${listId}/add-item`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + localStorage.getItem('token'),
                },
                body: JSON.stringify({
                    title: newItem.title,
                    description: newItem.description,
                    priority: newItem.priority,
                    isCompleted: newItem.isCompleted,
                }),
            });

            if (response.ok) {
                const result = await response.json();
                const created = result.data
                // Если сервер вернул объект с id — заменить временный элемент
                if (created && created.id) {
                    setLists((prev) =>
                        prev.map((l) => {
                            if (l.id !== listId) return l;
                            return {
                                ...l,
                                items: l.items.map((it) => (it.id === tempId ? created : it)),
                            };
                        })
                    );
                } else {
                    // Если сервер не вернул объект, просто оставить оптимистичный элемент
                    console.warn('Server did not return created item object');
                }
            } else {
                // Ошибка — убрать оптимистичный элемент
                console.warn('Failed to create item, status', response.status);
                setLists((prev) =>
                    prev.map((l) => {
                        if (l.id !== listId) return l;
                        return {
                            ...l,
                            items: l.items.filter((it) => it.id !== tempId),
                        };
                    })
                );
            }
        } catch (err) {
            console.error(err);
            // Откат
            setLists((prev) =>
                prev.map((l) => {
                    if (l.id !== listId) return l;
                    return {
                        ...l,
                        items: l.items.filter((it) => it.id !== tempId),
                    };
                })
            );
        } finally {
            // Сброс формы
            setForms((prev) => ({
                ...prev,
                [listId]: {
                    title: '',
                    description: '',
                    priority: 2,
                    isCompleted: false,
                    saving: false,
                },
            }));
        }
    };

    // Состояния загрузки и отсутствия данных
    if (loading && (!lists || lists.length === 0)) {
        return (
            <div style={styles.container}>
                <div style={styles.empty}>Загрузка...</div>
            </div>
        );
    }

    if (!lists || lists.length === 0) {
        return (
            <div style={styles.container}>
                {/* Форма создания списка при пустом состоянии */}
                <form
                    onSubmit={handleCreateList}
                    style={{ marginBottom: 12 }}
                    aria-label="Создать новый список"
                >
                    <div style={styles.formRow}>
                        <input
                            type="text"
                            placeholder="Название списка *"
                            value={newListName}
                            onChange={(e) => setNewListName(e.target.value)}
                            style={styles.input}
                            required
                        />
                        <button
                            type="submit"
                            style={{
                                ...styles.button,
                                ...(creatingList ? styles.buttonDisabled : {}),
                            }}
                            disabled={!newListName.trim() || creatingList}
                        >
                            {creatingList ? 'Создание...' : 'Создать список'}
                        </button>
                    </div>
                </form>

                <div style={styles.empty}>Нет доступных todo-списков.</div>
            </div>
        );
    }

    // Отрисовка списков и задач
    return (
        <div style={styles.container}>
            {lists.map((list) => (
                <section key={list.id} style={styles.listCard}>
                    <div style={styles.headerRow}>
                        <h2 style={styles.title}>{list.name}</h2>
                        <button
                            type="button"
                            onClick={() => handleDeleteList(list.id)}
                            style={{
                                ...styles.deleteButton,
                                ...(list._deleting ? styles.deleteButtonDisabled : {}),
                            }}
                            disabled={!!list._deleting}
                            aria-disabled={!!list._deleting}
                            aria-label={`Удалить список "${list.name}"`}
                        >
                            {list._deleting ? 'Удаляется...' : 'Удалить список'}
                        </button>
                    </div>

                    <table style={styles.table}>
                        <thead>
                            <tr>
                                <th style={styles.th}>Задача</th>
                                <th style={styles.th}>Описание</th>
                                <th style={styles.th}>Приоритет</th>
                                <th style={styles.th}>Выполнено</th>
                                <th style={styles.th}>Действия</th>
                            </tr>
                        </thead>
                        <tbody>
                            {(Array.isArray(list.items) && list.items.length > 0) ? (
                                list.items.map((item) => {
                                    const p = priorityLabel(item.priority);
                                    return (
                                        <tr key={item.id}>
                                            <td style={{ ...styles.td }}>
                                                <div style={item.isCompleted ? styles.completed : undefined}>
                                                    {item.title}
                                                </div>
                                            </td>
                                            <td style={styles.td}>{item.description || '—'}</td>
                                            <td style={styles.td}>
                                                <span
                                                    style={{
                                                        ...styles.priorityBadge,
                                                        background: p.color,
                                                    }}
                                                >
                                                    {p.text}
                                                </span>
                                            </td>
                                            <td
                                                style={{ ...styles.td, cursor: 'pointer', userSelect: 'none' }}
                                                role="button"
                                                tabIndex={0}
                                                onClick={() => handleToggleCompleted(list.id, item.id)}
                                                onKeyDown={(e) => {
                                                    if (e.key === 'Enter' || e.key === ' ') {
                                                        e.preventDefault();
                                                        handleToggleCompleted(list.id, item.id);
                                                    }
                                                }}
                                                aria-pressed={!!item.isCompleted}
                                                aria-label={`Отметить задачу "${item.title}" как ${item.isCompleted ? 'не выполненную' : 'выполненную'}`}
                                            >
                                                {item._saving ? 'Сохраняется...' : (item.isCompleted ? 'Да' : 'Нет')}
                                            </td>
                                            <td style={styles.td}>
                                                <button
                                                    type="button"
                                                    onClick={() => handleDeleteItem(list.id, item.id)}
                                                    style={{
                                                        ...styles.deleteButton,
                                                        ...(item._deleting ? styles.deleteButtonDisabled : {}),
                                                    }}
                                                    disabled={!!item._deleting}
                                                    aria-disabled={!!item._deleting}
                                                    aria-label={`Удалить задачу "${item.title}"`}
                                                >
                                                    {item._deleting ? 'Удаляется...' : 'Удалить'}
                                                </button>
                                            </td>
                                        </tr>
                                    );
                                })
                            ) : (
                                <tr>
                                    <td style={styles.td} colSpan={5}>
                                        Список пуст.
                                    </td>
                                </tr>
                            )}
                        </tbody>
                    </table>

                    {/* Форма добавления задачи */}
                    <form
                        onSubmit={(e) => handleAddItem(e, list.id)}
                        style={{ marginTop: 12 }}
                        aria-label={`Добавить задачу в список ${list.name}`}
                    >
                        <div style={styles.formRow}>
                            <input
                                type="text"
                                placeholder="Название задачи *"
                                value={(forms[list.id] && forms[list.id].title) || ''}
                                onChange={(e) => updateFormField(list.id, 'title', e.target.value)}
                                style={styles.input}
                                required
                            />
                            <select
                                value={(forms[list.id] && forms[list.id].priority) || 2}
                                onChange={(e) => updateFormField(list.id, 'priority', Number(e.target.value))}
                                style={styles.select}
                                aria-label="Приоритет"
                            >
                                <option value={1}>Low</option>
                                <option value={2}>Medium</option>
                                <option value={3}>High</option>
                            </select>
                            <label style={styles.checkboxLabel}>
                                <input
                                    type="checkbox"
                                    checked={!!(forms[list.id] && forms[list.id].isCompleted)}
                                    onChange={(e) => updateFormField(list.id, 'isCompleted', e.target.checked)}
                                />
                                Выполнено
                            </label>
                            <button
                                type="submit"
                                style={{
                                    ...styles.button,
                                    ...(forms[list.id] && forms[list.id].saving ? styles.buttonDisabled : {}),
                                }}
                                disabled={!(forms[list.id] && forms[list.id].title) || (forms[list.id] && forms[list.id].saving)}
                            >
                                {(forms[list.id] && forms[list.id].saving) ? 'Сохраняется...' : 'Добавить'}
                            </button>
                        </div>
                        <div style={styles.formRow}>
                            <textarea
                                placeholder="Описание (необязательно)"
                                value={(forms[list.id] && forms[list.id].description) || ''}
                                onChange={(e) => updateFormField(list.id, 'description', e.target.value)}
                                style={styles.textarea}
                            />
                        </div>
                        <div style={styles.smallNote}>Поля помеченные * обязательны.</div>
                    </form>
                </section>
            ))}


            {/* Форма создания нового списка */}
            <form
                onSubmit={handleCreateList}
                style={{ marginBottom: 12 }}
                aria-label="Создать новый список"
            >
                <div style={styles.formRow}>
                    <input
                        type="text"
                        placeholder="Название списка *"
                        value={newListName}
                        onChange={(e) => setNewListName(e.target.value)}
                        style={styles.input}
                        required
                    />
                    <button
                        type="submit"
                        style={{
                            ...styles.button,
                            ...(creatingList ? styles.buttonDisabled : {}),
                        }}
                        disabled={!newListName.trim() || creatingList}
                    >
                        {creatingList ? 'Создание...' : 'Создать список'}
                    </button>
                </div>
            </form>


        </div>

    );
};

export default TodoList;
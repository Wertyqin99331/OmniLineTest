import React, { useState } from 'react';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import {IContact} from "../models/IContact.ts";
import {toast} from "react-toastify";

interface IEditContactModalProps {
    selectedContact: IContact
    onClose: () => void;
}

const EditContact: React.FC<IEditContactModalProps> = ({ selectedContact, onClose }) => {
    const [email, setEmail] = useState(selectedContact.email);
    const [fullName, setFullName] = useState(selectedContact.fullName);

    const queryClient = useQueryClient();

    const updateContactMutation = useMutation({
        mutationFn: async () => {
            const response = await fetch(`http://localhost:5103/contacts/${selectedContact.id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({email, fullName}),
            });
            if (!response.ok) {
                throw new Error('Не удалось обновить контакт');
            }
        },
        onSuccess: () => {
            queryClient.setQueryData(['contacts'], (oldData: IContact[]) => {
                if (oldData) {
                    return oldData.map((contact) => {
                        if (contact.id === selectedContact.id) {
                            return {
                                ...contact,
                                email,
                                fullName,
                            };
                        }
                        return contact;
                    });
                }
                return oldData;
            })
            toast.success("Контакт успешно обновлен")
            onClose()
        },
        onError: (error) => {
            toast.error(error.message)
        }
    });

    // Handle form submission
    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();


        updateContactMutation.mutate();
    };

    return (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
            <div className="bg-white p-6 rounded-lg shadow-lg w-96">
                <h2 className="text-xl font-bold mb-4">Редактировать контакт</h2>
                <form onSubmit={handleSubmit} className="space-y-4">
                    <div>
                        <label htmlFor="email" className="block text-sm font-medium text-gray-700">
                            Почта
                        </label>
                        <input
                            type="email"
                            id="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            className="mt-1 block w-full p-2 border border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500"
                            required
                        />
                    </div>
                    <div>
                        <label htmlFor="fullName" className="block text-sm font-medium text-gray-700">
                            Full Name
                        </label>
                        <input
                            type="text"
                            id="fullName"
                            value={fullName}
                            onChange={(e) => setFullName(e.target.value)}
                            className="mt-1 block w-full p-2 border border-gray-300 rounded-md shadow-sm focus:ring-blue-500 focus:border-blue-500"
                            required
                        />
                    </div>
                    <div className="flex justify-end space-x-2">
                        <button
                            type="button"
                            onClick={onClose}
                            className="px-4 py-2 bg-gray-500 text-white rounded-md hover:bg-gray-600"
                        >
                            Закрыть
                        </button>
                        <button
                            type="submit"
                            className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
                            disabled={updateContactMutation.isPending}
                        >
                            {updateContactMutation.isPending ? 'Сохранение...' : 'Сохранить'}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default EditContact;

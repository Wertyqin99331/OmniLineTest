import {useMutation, useQuery, useQueryClient} from "@tanstack/react-query";
import {ICounterpart} from "./models/ICounterpart.ts";
import {toast} from "react-toastify";
import {useState} from "react";
import {IContact} from "./models/IContact.ts";
import EditContact from "./components/EditContact.tsx";

function App() {
    const queryClient = useQueryClient()

    const [name, setName] = useState<string>()
    const [selectedCounterpart, setSelectedCounterpart] = useState<ICounterpart | undefined>()

    const {data: counterparts, isLoading: isCounterpartsLoading} = useQuery({
        queryKey: ['counterparts'],
        queryFn: async () => fetch("http://localhost:5103/counterparts").then(res => res.json() as Promise<ICounterpart[]>)
    })

    const addCounterpartMutation = useMutation({
        mutationFn: async (name: string) => {
            const res = await fetch("http://localhost:5103/counterparts", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({name})
            })
            const text: string = await res.text()
            if (res.ok) {
                const newCounterpart: ICounterpart = JSON.parse(text)
                queryClient.setQueryData<ICounterpart[]>(['counterparts'], (oldData) => {
                    return oldData ? [...oldData, newCounterpart] : [newCounterpart];
                });
            } else {
                throw new Error(text)
            }
        },
        onSuccess: () => {
            setName("")
            toast.success("Контрагент успешно добавлен")
        },
        onError: (e) => {
            toast.error(e.message)
        }
    })

    const addCounterpartHandler = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()

        const name = (e.currentTarget.elements.namedItem('name') as HTMLInputElement).value;

        addCounterpartMutation.mutate(name)
    }

    const [email, setEmail] = useState<string>()
    const [fullName, setFullName] = useState<string>()

    const [selectedContact, setSelectedContact] = useState<IContact | undefined>()

    const {data: contacts, isLoading: isContactsLoading} = useQuery({
        queryKey: ['contacts'],
        queryFn: async () => fetch("http://localhost:5103/contacts").then(res => res.json() as Promise<IContact[]>)
    })


    const addContactMutation = useMutation({
        mutationFn: async ({email, fullName}: { email: string, fullName: string }) => {
            const res = await fetch("http://localhost:5103/contacts", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({email, fullName, counterpartId: selectedCounterpart!.id})
            })
            const text: string = await res.text()
            if (res.ok) {
                const newContact: IContact = JSON.parse(text)
                queryClient.setQueryData<IContact[]>(['contacts'], (oldData) => {
                    return oldData ? [...oldData, newContact] : [newContact];
                });
            } else {
                throw new Error(text)
            }
        },
        onSuccess: () => {
            setEmail("")
            setFullName("")
            toast.success("Контакт успешно добавлен")
        },
        onError: (e) => {
            toast.error(e.message)
        }
    })

    const deleteContactMutation = useMutation({
        mutationFn: async (id: string) => {
            const res = await fetch('http://localhost:5103/contacts/' + id, {
                method: 'DELETE',
                headers: {
                    "Content-Type": "application/json"
                }
            })
            if (!res.ok) {
                const text = await res.text()
                throw new Error(text)
            } else {
                queryClient.setQueryData(['contacts'], (oldData: IContact[] | undefined) => oldData?.filter(c => c.id !== id))
            }
        },
        onSuccess: () => {
            toast.success("Контакт успешно удален")
        },
        onError: () => {
            toast.error("Не удалось удалить контакт")
        }
    })

    const addContactHandler = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()

        const email = (e.currentTarget.elements.namedItem('email') as HTMLInputElement).value;
        const fullName = (e.currentTarget.elements.namedItem('full_name') as HTMLInputElement).value;

        addContactMutation.mutate({email, fullName})
    }

    return (
        <>
            <div className="grid grid-cols-2">
                <div className="p-4 flex flex-col gap-y-4">
                    <h2>Контрагенты</h2>
                    <form className="border border-black rounded-md p-4 flex flex-col gap-y-3 items-start mt-5"
                          onSubmit={addCounterpartHandler}>
                        <h3>Добавить контрагента</h3>
                        <div className="flex gap-x-2 items-center">
                            <label htmlFor="name" className="cursor-pointer">Имя</label>
                            <input type="text" id="name" className="p-1 border border-black rounded-md" value={name}
                                   onChange={(e) => setName(e.target.value)} name="name"/>
                        </div>
                        <button
                            className="bg-blue-600 text-white px-3 py-1 rounded-md hover:bg-blue-800 transition-colors"
                            type="submit" disabled={addCounterpartMutation.isPending}>Добавить контрагента
                        </button>
                    </form>
                    <ul className="flex flex-col gap-y-2">
                        {isContactsLoading && <li>Загрузка...</li>}
                        {counterparts?.map(counterpart => (
                            <li key={counterpart.id} className={`p-2 border border-black rounded-md 
                        ${selectedCounterpart?.id == counterpart.id ? 'bg-gray-200' : ''}`}>
                                <button
                                    onClick={() => {
                                        if (selectedCounterpart?.id == counterpart.id) {
                                            setSelectedCounterpart(undefined)
                                        } else {
                                            setSelectedCounterpart(counterpart)
                                        }
                                    }}
                                    type="button" className="w-full text-left">{counterpart.name}</button>
                            </li>
                        ))}
                    </ul>
                </div>
                <div className="p-4">
                    <h2>Контакты</h2>
                    <form className="border border-black rounded-md p-4 flex flex-col gap-y-3 items-start mt-5"
                          onSubmit={addContactHandler}>
                        <h3>Добавить контакт</h3>
                        <div className="flex gap-x-2 items-center">
                            <label htmlFor="email" className="cursor-pointer">Почта</label>
                            <input type="email" id="email" className="p-1 border border-black rounded-md" value={email}
                                   onChange={(e) => setEmail(e.target.value)} name="email"/>
                        </div>
                        <div className="flex gap-x-2 items-center">
                            <label htmlFor="full_name" className="cursor-pointer">Имя</label>
                            <input type="text" id="full_name" className="p-1 border border-black rounded-md"
                                   value={fullName}
                                   onChange={(e) => setFullName(e.target.value)} name="full_name"/>
                        </div>
                        <button
                            className="bg-blue-600 text-white px-3 py-1 rounded-md hover:bg-blue-800 transition-colors disabled:bg-blue-200"
                            type="submit"
                            disabled={addCounterpartMutation.isPending || selectedCounterpart === undefined}>Добавить
                            контакт
                        </button>
                    </form>
                    <ul className='flex flex-col gap-y-2'>
                        {isCounterpartsLoading && <li>Загрузка...</li>}
                        {contacts?.filter(c => selectedCounterpart === undefined || c.counterpart.id == selectedCounterpart?.id).map(contact => (
                            <li key={contact.id}
                                className='p-2 border border-black rounded-md flex gap-x-2 items-center'>
                                <span>{contact.email}</span>
                                <span>{contact.fullName}</span>
                                <button type='button'
                                        className='bg-blue-600 text-white px-3 py-1 rounded-md hover:bg-blue-800'
                                        onClick={() => setSelectedContact(contact)}>Редактировать контакт
                                </button>
                                <button type='button' onClick={() => deleteContactMutation.mutate(contact.id)}
                                        className='bg-red-600 text-white px-3 py-1 rounded-md hover:bg-red-800'>Удалить
                                    контакт
                                </button>
                            </li>
                        ))}
                    </ul>
                </div>
            </div>

            {selectedContact &&
                <EditContact selectedContact={selectedContact} onClose={() => setSelectedContact(undefined)}/>}
        </>

    )
}

export default App

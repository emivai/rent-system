import { Profile } from "@/pages/account";
import { Dispatch, SetStateAction, createContext, useContext, useEffect, useState } from "react";

export type LoadingState = 'loading' | 'loggedin' | 'loggedout';

export interface User {
    token: string,
    exp?: number,
    profile: Profile
}

export interface Exposed {
    user: User | null,
    setUser: Function,
    loadingState: LoadingState
}

const Context = createContext<any>(null);

const Provider = ({ children }: any) => {
    const [user, setUser] = useState<User | null>(null);
    const [loading, setLoading] = useState<LoadingState>('loading');

    function getUserFromStorage() {
        return localStorage.getItem('user') ? JSON.parse(localStorage.getItem('user') || '{}') : {};
    }

    function getLoadingState(curr: User) {
        let ls: LoadingState;
        if (curr) {
            if (curr.exp && curr.exp > new Date().getTime()) {
                ls = 'loggedin';
            } else {
                ls = 'loggedout';
            }
        } else {
            ls = 'loading';
        }
        return ls;
    }

    useEffect(() => {
        const tuser = getUserFromStorage();
        setLoading(getLoadingState(tuser));
        if (!tuser) return;
        if (tuser.exp > new Date().getTime())
            setUser(getUserFromStorage())
        else
            localStorage.removeItem('user')
    }, [])

    const changeUser = (user: User | null) => {
        if (!user) {
            setUser(null);
            setLoading('loggedout');
            localStorage.removeItem('user');
        } else {
            localStorage.setItem('user', JSON.stringify({ ...user, exp: (new Date().getTime() + 1000 * 60 * 60) }));
            setLoading('loggedin');
            setUser(user);
        }
    }

    const exposed: Exposed = {
        user,
        setUser: changeUser,
        loadingState: loading
    }
    return (
        <Context.Provider value={exposed}>
            {children}
        </Context.Provider>
    )
}

export const useUser = () => useContext<Exposed>(Context);

export default Provider;
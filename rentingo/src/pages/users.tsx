import { useUser } from "@/context/user";
import { useRouter } from "next/router";
import { useEffect, useState } from "react";
import { Profile, getRole } from "./account";
import { getAllUsers } from "@/controllers/UserController";
import Spinner from "@/components/Spinner";
import UserCard from "@/components/UserCard";

export default function UsersPage() {
    const { loadingState, user } = useUser();
    const router = useRouter();
    const [users, setUsers] = useState<Profile[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        if (loadingState === 'loading') return;
        if (loadingState === 'loggedout') {
            router.push('/login');
            return;
        }
        if (getRole(user) !== 'administratorius') {
            router.push('/');
            return;
        }
        async function fetchInitial() {
            const usrs = await getAllUsers();
            setUsers(usrs);
            setLoading(false);
        }
        fetchInitial();
    }, [loadingState]);

    return (
        <div className="grid gap-2 justify-center">
            {loading ? <Spinner /> : <>
                {users.length === 0 ? <p className="text-red-500">Nėra vartotojų</p> :
                    users.map((curr, ind) => <UserCard key={`usr${ind}`} admin={user} profile={curr} />)}
            </>}
        </div>
    )
}
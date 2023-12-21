import Spinner from "@/components/Spinner";
import { useUser } from "@/context/user"
import { Contract, getUserContracts } from "@/controllers/ContractController";
import { useRouter } from "next/router";
import { useEffect, useState } from "react"


export default function ContractsPage() {
    const [contracts, setContracts] = useState<Contract[]>([]);
    const [loading, setLoading] = useState(true);
    const { loadingState, user } = useUser();
    const router = useRouter();
    useEffect(() => {
        if (loadingState === 'loading') return;
        if (loadingState === 'loggedout' || !user) router.push("/login");
        async function getContracts() {
            setContracts(await getUserContracts(user));
            setLoading(false);
        }
        if (loadingState === 'loggedin') {
            getContracts();
        }
    }, [router, loadingState]);

    return (
        <div className="grid justify-center gap-2 mt-2">
            {loading ? <Spinner /> : <>
                {contracts.length > 0 ? contracts.map(w => <div key={w.id} className="border border-gray-600 rounded-xl p-4">
                    <p className="font-bold text-xl">{w.id}. {w.item.name}</p>
                    <p>kaina: {w.price} €</p>
                    <p>Nuomotojo El. paštas: </p>
                    <p>{w.owner.email}</p>
                    <p>Savininko El. paštas: </p>
                    <p>{w.renter.email}</p>
                </div>) : <p>Kontraktų neturite</p>}
            </>}
        </div>
    )
}
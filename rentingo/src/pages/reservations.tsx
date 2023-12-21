import Spinner from "@/components/Spinner";
import { useUser } from "@/context/user";
import { Reservation, getReservations } from "@/controllers/ReservationController";
import { useRouter } from "next/router";
import { useEffect, useState } from "react";

export default function ReservationsPage() {
    const [reservations, setReservations] = useState<Reservation[]>([]);
    const { loadingState, user } = useUser();
    const [loading, setLoading] = useState(true);
    const router = useRouter();
    useEffect(() => {
        if (loadingState === 'loading') return;
        if (loadingState === 'loggedout' || !user) router.push("/login");
        async function fetchRs() {
            try {
                const rs = await getReservations();
                setReservations(rs.filter(w => w.user.id === user?.profile.id));
            } catch (rr) {
                console.log(rr);
            } finally {
                setLoading(false);
            }
        }
        fetchRs();
    }, [loadingState]);
    return (
        <div className="grid justify-center gap-2">
            {loading ? <Spinner /> : <>
                <h1 className="text-center text-xl py-2">Rezervacijos</h1>
                {reservations.length === 0 ? "Nera rezervaciju" : <div>
                    <table className="w-full text-sm text-left text-gray-500">
                        <thead className="text-xs text-gray-700 uppercase bg-gray-50">
                            <tr>
                                <th className="px-6 py-3">Nuo</th>
                                <th className="px-6 py-3">Iki</th>
                                <th className="px-6 py-3">Kaina</th>
                                <th className="px-6 py-3">Daiktas</th>
                            </tr>
                        </thead>
                        <tbody>
                            {reservations.map(rsr => <tr key={rsr.id} className="bg-white border-b">
                                <td className="px-6 py-4">{rsr.dateFrom.split("T")[0]}</td>
                                <td className="px-6 py-4">{rsr.dateTo.split("T")[0]}</td>
                                <td className="px-6 py-4">{rsr.price} €</td>
                                <td className="px-6 py-4">{rsr.item.name}</td>
                            </tr>)}
                        </tbody>
                    </table>
                </div>}
            </>}
        </div>
    )
}
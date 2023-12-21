import { Item } from "@/components/Post";
import { User } from "@/context/user";
import { Profile } from "@/pages/account";
import { GetDays } from "@/utils/utils";

export interface Reservation {
    dateFrom: string,
    dateTo: string,
    id?: number,
    item: Item,
    price: number,
    user: Profile
}

export async function reserveItem(item: Item, price: number, from: string, to: string, user: User | null) {
    const fDate = new Date(from);
    const tDate = new Date(to);
    const days = GetDays(fDate, tDate);
    const rs = await fetch(`${process.env.NEXT_PUBLIC_API}/api/reservations`, {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${user?.token}`
        },
        body: JSON.stringify({
            dateFrom: fDate.toISOString(),
            dateTo: tDate.toISOString(),
            price,
            itemId: item.id
        })
    })
    if (rs.ok) {
        return { success: true, msg: "" };
    } else {
        return { success: false, msg: "Rezervacija jau egzistuoja" }
    }
}

export async function getReservations(): Promise<Reservation[]>{
    const rs = await fetch(`${process.env.NEXT_PUBLIC_API}/api/reservations`)
    return await rs.json();
}
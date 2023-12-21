import { Item } from "@/components/Post";
import { User } from "@/context/user";
import { Profile, getRole } from "@/pages/account";


export interface Contract {
    id: string,
    date: string,
    rentLength: number,
    price: number,
    item: Item,
    renter: Profile,
    owner: Profile
}

export async function postContract(rentLength: string, item: Item, user: User | null) {
    try {
        const rl = parseInt(rentLength);
        const rp = parseFloat(item.price);
        const prc = Math.round(rl * rp * 100) / 100;
        const rs = await fetch(`${process.env.NEXT_PUBLIC_API}/api/contracts`, {
            method: "POST",
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${user?.token}`
            },
            body: JSON.stringify({
                rentLenght: rl,
                price: prc,
                itemId: item.id
            })
        });
        if (!rs.ok)
            return { succ: false, msg: "Nepavyko užregistruoti" };
        return { succ: true, msg: "Sėkmingai užregistruota" };
    } catch (rr) {
        console.log(rr);
        return { succ: false, msg: "Nepavyko užregistruoti" };
    }
}

export async function getUserContracts(user: User | null): Promise<Contract[]> {
    const rs = await fetch(`${process.env.NEXT_PUBLIC_API}/api/contracts`);
    const json: Contract[] = await rs.json();
    if (getRole(user) === 'administratorius')
        return json;
    return json.filter(w => w.renter.id === user?.profile.id || w.owner.id == user?.profile.id);
}
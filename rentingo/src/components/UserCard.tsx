import { User } from "@/context/user";
import { getRoleName, setUser } from "@/controllers/UserController";
import { getRole } from "@/pages/account";
import { Profile } from "@/pages/account";
import Head from "next/head";
import { useState } from "react";

interface Props {
    profile: Profile,
    admin: User | null
}


export default function UserCard({ admin, profile }: Props) {
    const [changing, setChanging] = useState(false);
    const [prof, setProf] = useState(profile);
    const [tmpProf, setTmpProf] = useState(profile);

    function CancelEdit() {
        setChanging(false);
        setTmpProf(prof);
    }

    async function ChangeUser() {
        const succ = await setUser(tmpProf, admin?.token);
        if (succ) {
            setProf(tmpProf)
            setChanging(false);
        } else {
            alert("Nepavyko pakeisti!");
        }
    }

    return (
        <div className="border-blue-300 border rounded-lg p-3 hover:bg-blue-100">
            <Head>
                <title>Vartotojai</title>
            </Head>
            {changing ?
                <div>
                    <p>El. paštas</p>
                    <input type="email" name="email" id="email" value={tmpProf.email} onChange={w => setTmpProf(curr => ({ ...curr, email: w.target.value }))} />
                </div> : <p className="text-xl font-bold">{prof.id}. {prof.email}</p>}
            {changing ? <div>
                <p>Vardas</p>
                <input type="text" name="name" id="name" value={tmpProf.email} onChange={w => setTmpProf(curr => ({ ...curr, email: w.target.value }))} />
            </div> : <p>{prof.name} {prof.surname}</p>}
            {changing ? <div className="flex gap-2">
                <p>Role</p>
                <select defaultValue={tmpProf.role} onChange={w => setTmpProf(curr => ({ ...curr, role: parseInt(w.target.value) }))} name="rol" id="rol">
                    <option value="1">Nuomotojas</option>
                    <option value="2">Nuomininkas</option>
                </select>
            </div> : <p>Rolė: {getRoleName(profile)}</p>}
            {changing ? <div className="flex gap-3">

                <div>
                    <p>Miestas</p>
                    <input type="text" name="city" id="city" value={tmpProf.city} onChange={w => setTmpProf(curr => ({ ...curr, city: w.target.value }))} />
                </div>
                <div>
                    <p>Gatvė</p>
                    <input type="text" name="street" id="street" value={tmpProf.street} onChange={w => setTmpProf(curr => ({ ...curr, street: w.target.value }))} />
                </div>
                <div>
                    <p>Namo numeris</p>
                    <input type="text" name="house" id="house" value={tmpProf.houseNumber} onChange={w => setTmpProf(curr => ({ ...curr, houseNumber: w.target.value }))} />
                </div>
            </div> : <p>{prof.city} {prof.street} {prof.houseNumber}</p>}
            {changing ? <div>
                <p>Pašto kodas</p>
                <input type="text" name="post" id="post" value={tmpProf.postCode} onChange={w => setTmpProf(curr => ({ ...curr, postCode: w.target.value }))} />
            </div> : <p>{prof.postCode}</p>}
            {changing ? <div>
                <p>Telefonas</p>
                <input type="text" name="phone" id="phone" value={tmpProf.phone} onChange={w => setTmpProf(curr => ({ ...curr, phone: w.target.value }))} />
            </div> : <p>{prof.phone}</p>}
            <div className="flex gap-2 mt-3">
                {!changing ? <button className="border-gray-500 border px-1 py-0.5 rounded-lg hover:bg-gray-500 text-black hover:text-white transition-colors" onClick={() => setChanging(true)}>Keisti</button> : <>
                    <button className="border-gray-500 border px-1 py-0.5 rounded-lg hover:bg-gray-500 text-black hover:text-white transition-colors" onClick={() => CancelEdit()}>Atšaukti</button>
                    <button className="border-gray-500 border px-1 py-0.5 rounded-lg hover:bg-gray-500 text-black hover:text-white transition-colors" onClick={() => ChangeUser()}>Pateikti</button>
                </>}
            </div>
        </div>
    )
}
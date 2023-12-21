import Spinner from '@/components/Spinner';
import { User, useUser } from '@/context/user';
import Head from 'next/head';
import { useRouter } from 'next/router';
import { FormEvent, useEffect, useState } from 'react';

export type Role = 'nuomotojas' | 'nuomininkas' | 'administratorius';

export function getRole(user: User | null) : Role{
    const rl: any = user?.profile.role;
    if(rl === 0){
        return "administratorius";
    } else if(rl === 2) {
        return "nuomininkas";
    } else {
        return "nuomotojas";
    }
}

export interface Profile {
    role: number,
    name: string,
    surname: string,
    phone: string,
    email: string,
    city: string,
    street: string,
    houseNumber: string,
    postCode: string,
    id?: number
}

export default function Profile() {
    const [profile, setProfile] = useState<Profile>();
    const [status, setStatus] = useState<{ val: number, msg: string }>({ val: 0, msg: "" });
    const [errors, setErrors] = useState<string[]>([]);
    const [emsg, setEMsg] = useState<string[]>([]);
    const [saveState, setSaveState] = useState<{ loading: boolean, msg: string }>({ loading: false, msg: "" });
    const { loadingState, user } = useUser();
    const router = useRouter();
    useEffect(() => {
        async function fetchProfile() {
            try {
                const rs = await fetch(`${process.env.NEXT_PUBLIC_API}/api/auth/currentUser`, {
                    headers: {
                        'Authorization': `Bearer ${user?.token}`
                    }
                })
                const json = await rs.json();
                setProfile(json);
                setStatus({ val: 2, msg: "" });
            } catch (rr) {
                console.log(rr);
                setStatus({ val: 1, msg: "Nepavyko gauti vartotojo duomenų" });
            }
        }
        if (loadingState === 'loading') return;
        if (loadingState === 'loggedout') router.push("/login");
        else {
            fetchProfile();
        }
    }, [loadingState])


    async function changeProfile(w: FormEvent<HTMLFormElement>) {
        w.preventDefault();
        if (!profile) return;
        if (saveState.loading) return;
        setSaveState({ loading: true, msg: "" });
        setEMsg([]);
        setErrors([]);
        const lettersOnly = /^[a-zA-Ząčęėįšųūž]+$/;
        const phoneReg = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/;
        const mailReg = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/g;
        let rorrs = [];
        let rnames = [];

        if (!lettersOnly.test(profile.name)) {
            rorrs.push("Blogas vardo formatas");
            rnames.push('name');
        }
        if (!lettersOnly.test(profile.surname)) {
            rorrs.push("Blogas pavardės formatas");
            rnames.push('surname');
        }
        if (!phoneReg.test(profile.phone)) {
            rorrs.push("Blogas telefono numerio formatas");
            rnames.push('phone');
        }
        if (!mailReg.test(profile.email)) {
            rorrs.push("Blogas el. pašto formatas");
            rnames.push('email');
        }
        if (rorrs.length > 0) {
            setEMsg(rorrs);
            setErrors(rnames);
            setSaveState({ loading: false, msg: "Nepavyko išsaugoti dėl klaidų:" });
            return;
        }

        try {
            const rs = await fetch(`${process.env.NEXT_PUBLIC_API}/api/users`, {
                method: "PUT",
                headers: {
                    'Authorization': `Bearer ${user?.token}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(profile)
            })
            if (rs.ok) {
                setSaveState({ loading: false, msg: "Išsaugota sėkmingai" });
            } else {
                setSaveState({ loading: false, msg: "Nepavyko išsaugoti." });
            }
        } catch (rr) {
            console.log(rr);
            setSaveState({ loading: false, msg: "Nepavyko išsaugoti." });
        }
    }

    return (
        <div>
            <Head>
                <title>Profilis</title>
                <meta name="description" content="Renting system" />
                <meta name="viewport" content="width=device-width, initial-scale=1" />
                <link rel="icon" href="/favicon.ico" />
            </Head>
            {status.val === 0 ? <div className='flex justify-center mt-2'>
                <Spinner />
            </div> : status.val === 1 ?
                <div className='flex justify-center mt-2 text-red-500'>
                    {status.msg}
                </div> :
                <div className='max-w-[600px] m-auto px-4'>
                    <form onSubmit={w => changeProfile(w)} className="py-8 text-base leading-6 grid gap-4 text-gray-700 sm:text-lg sm:leading-7">
                        <div className="relative">
                            <input onChange={w => setProfile(curr => curr ? ({ ...curr, name: w.target.value }) : curr)} value={profile?.name} autoComplete="off" id="vardas" name="vardas" type="text" className={`peer placeholder-transparent h-10 w-full border-b-2 ${errors.includes("name") ? 'border-red-500' : 'border-gray-300'} text-gray-900 focus:outline-none focus:borer-rose-600`} placeholder="Vardas" />
                            <label htmlFor="vardas" className="absolute left-0 -top-3.5 text-gray-600 text-sm peer-placeholder-shown:text-base peer-placeholder-shown:text-gray-440 peer-placeholder-shown:top-2 transition-all peer-focus:-top-3.5 peer-focus:text-gray-600 peer-focus:text-sm">Vardas</label>
                        </div>
                        <div className="relative">
                            <input onChange={w => setProfile(curr => curr ? ({ ...curr, surname: w.target.value }) : curr)} value={profile?.surname} autoComplete="off" id="surname" name="surname" type="text" className={`peer placeholder-transparent h-10 w-full border-b-2 ${errors.includes("surname") ? 'border-red-500' : 'border-gray-300'} text-gray-900 focus:outline-none focus:borer-rose-600`} placeholder="Pavardė" />
                            <label htmlFor="surname" className="absolute left-0 -top-3.5 text-gray-600 text-sm peer-placeholder-shown:text-base peer-placeholder-shown:text-gray-440 peer-placeholder-shown:top-2 transition-all peer-focus:-top-3.5 peer-focus:text-gray-600 peer-focus:text-sm">Pavardė</label>
                        </div>
                        <div className="relative">
                            <input onChange={w => setProfile(curr => curr ? ({ ...curr, phone: w.target.value }) : curr)} value={profile?.phone} autoComplete="off" id="phone" name="phone" type="text" className={`peer placeholder-transparent h-10 w-full border-b-2 ${errors.includes("phone") ? 'border-red-500' : 'border-gray-300'} text-gray-900 focus:outline-none focus:borer-rose-600`} placeholder="Telefono nr." />
                            <label htmlFor="phone" className="absolute left-0 -top-3.5 text-gray-600 text-sm peer-placeholder-shown:text-base peer-placeholder-shown:text-gray-440 peer-placeholder-shown:top-2 transition-all peer-focus:-top-3.5 peer-focus:text-gray-600 peer-focus:text-sm">Telefono nr.</label>
                        </div>
                        <div className="relative">
                            <input onChange={w => setProfile(curr => curr ? ({ ...curr, email: w.target.value }) : curr)} value={profile?.email} autoComplete="off" id="email" name="email" type="email" className={`peer placeholder-transparent h-10 w-full border-b-2 ${errors.includes("email") ? 'border-red-500' : 'border-gray-300'} text-gray-900 focus:outline-none focus:borer-rose-600`} placeholder="El. paštas" />
                            <label htmlFor="email" className="absolute left-0 -top-3.5 text-gray-600 text-sm peer-placeholder-shown:text-base peer-placeholder-shown:text-gray-440 peer-placeholder-shown:top-2 transition-all peer-focus:-top-3.5 peer-focus:text-gray-600 peer-focus:text-sm">El. paštas</label>
                        </div>
                        <div className="relative">
                            <input onChange={w => setProfile(curr => curr ? ({ ...curr, city: w.target.value }) : curr)} value={profile?.city} autoComplete="off" id="city" name="city" type="text" className={`peer placeholder-transparent h-10 w-full border-b-2 ${errors.includes("city") ? 'border-red-500' : 'border-gray-300'} text-gray-900 focus:outline-none focus:borer-rose-600`} placeholder="Miestas" />
                            <label htmlFor="city" className="absolute left-0 -top-3.5 text-gray-600 text-sm peer-placeholder-shown:text-base peer-placeholder-shown:text-gray-440 peer-placeholder-shown:top-2 transition-all peer-focus:-top-3.5 peer-focus:text-gray-600 peer-focus:text-sm">Miestas</label>
                        </div>
                        <div className="relative">
                            <input onChange={w => setProfile(curr => curr ? ({ ...curr, street: w.target.value }) : curr)} value={profile?.street} autoComplete="off" id="street" name="street" type="text" className={`peer placeholder-transparent h-10 w-full border-b-2 ${errors.includes("street") ? 'border-red-500' : 'border-gray-300'} text-gray-900 focus:outline-none focus:borer-rose-600`} placeholder="Gatvė" />
                            <label htmlFor="street" className="absolute left-0 -top-3.5 text-gray-600 text-sm peer-placeholder-shown:text-base peer-placeholder-shown:text-gray-440 peer-placeholder-shown:top-2 transition-all peer-focus:-top-3.5 peer-focus:text-gray-600 peer-focus:text-sm">Gatvė</label>
                        </div>
                        <div className="relative">
                            <input onChange={w => setProfile(curr => curr ? ({ ...curr, houseNumber: w.target.value }) : curr)} value={profile?.houseNumber} autoComplete="off" id="houseNumber" name="houseNumber" type="text" className={`peer placeholder-transparent h-10 w-full border-b-2 ${errors.includes("houseNumber") ? 'border-red-500' : 'border-gray-300'} text-gray-900 focus:outline-none focus:borer-rose-600`} placeholder="Namo nr." />
                            <label htmlFor="houseNumber" className="absolute left-0 -top-3.5 text-gray-600 text-sm peer-placeholder-shown:text-base peer-placeholder-shown:text-gray-440 peer-placeholder-shown:top-2 transition-all peer-focus:-top-3.5 peer-focus:text-gray-600 peer-focus:text-sm">Namo nr.</label>
                        </div>
                        <div className="relative">
                            <input onChange={w => setProfile(curr => curr ? ({ ...curr, postCode: w.target.value }) : curr)} value={profile?.postCode} autoComplete="off" id="postCode" name="postCode" type="text" className={`peer placeholder-transparent h-10 w-full border-b-2 ${errors.includes("postCode") ? 'border-red-500' : 'border-gray-300'} text-gray-900 focus:outline-none focus:borer-rose-600`} placeholder="Pašto kodas" />
                            <label htmlFor="postCode" className="absolute left-0 -top-3.5 text-gray-600 text-sm peer-placeholder-shown:text-base peer-placeholder-shown:text-gray-440 peer-placeholder-shown:top-2 transition-all peer-focus:-top-3.5 peer-focus:text-gray-600 peer-focus:text-sm">Pašto kodas</label>
                        </div>
                        <div className="relative">
                            {saveState.loading ?
                                <Spinner /> :
                                <button className="bg-blue-500 text-white rounded-md px-2 py-1">Keisti</button>}
                        </div>
                        {!saveState.loading && saveState.msg !== "" ? <p>{saveState.msg}</p> : ""}
                        {emsg.length > 0 ? <div>{emsg.map((w: string, ind: number) => (
                            <p key={`err-${ind}`} className="text-red-500">{w}</p>
                        ))}</div> : ""}
                    </form>
                </div>}
        </div>
    )
}
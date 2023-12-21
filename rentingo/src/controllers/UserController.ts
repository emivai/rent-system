import { User } from "@/context/user";
import { Profile, getRole } from "@/pages/account";

export async function getAllUsers() {
    const rs = await fetch(`${process.env.NEXT_PUBLIC_API}/api/users`);
    let users: Profile[] = await rs.json()
    return users;
}

export function getRoleName(profile: Profile) {
    const usr: User = { profile, token: "" };
    switch (getRole(usr)) {
        case "administratorius":
            return "Administratorius";
        case "nuomininkas":
            return "Nuomininkas";
        case "nuomotojas":
            return "Nuomotojas";
        default:
            return "Nezinoma rolė";
    }
}

export async function setUser(user: Profile, token?: string) {
    const tmpUser = { ...user };
    delete tmpUser.id;
    const rs = await fetch(`${process.env.NEXT_PUBLIC_API}/api/users/${user.id}`, {
        method: "PUT",
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(tmpUser)
    });
    if (rs.ok)
        return true;
    return false;
}

export async function loginFunc(email: string, password: string, setUser: Function, router: any, setSucc: Function) {
    try {
        const rs = await fetch(`${process.env.NEXT_PUBLIC_API}/api/auth/login`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'accept': '*/*'
            },
            body: JSON.stringify({ email, password })
        });
        const json = await rs.json();
        if (rs.ok) {
            const rs2 = await fetch(`${process.env.NEXT_PUBLIC_API}/api/auth/currentUser`, {
                headers: {
                    'Authorization': `Bearer ${json.accessToken}`
                }
            })
            const user = await rs2.json();
            setUser({ token: json.accessToken, profile: user })
            router.push('/');
        } else {
            setSucc({ val: 2, msg: "Nepavyko prisijungti" });
        }
    } catch (rr) {
        console.log(rr);
        setSucc({ val: 2, msg: "Nepavyko prisijungti" });
    }
}

export async function registerFunc(email: any, password: any, rol: any, setSucc: Function, router: any) {
    try {
        const rs = await fetch(`${process.env.NEXT_PUBLIC_API}/api/auth/register`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'accept': '*/*'
            },
            body: JSON.stringify({ email: email.val, password: password.val, role: rol })
        })
        if (rs.ok) {
            setSucc({ val: 1, msg: ["Sekmingai prisiregistruota!"] });
            router.push('/login');
        } else {
            const json = await rs.json();
            let msg = "Nepavyko prisiregistruoti";
            if (json.message && json.message.includes("exists")) {
                msg = "Toks el. paštas jau egzistuoja";
            }
            setSucc({ val: 2, msg: [msg] });
        }
    } catch (rr) {
        console.log(rr);
    }
}
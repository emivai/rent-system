import { useUser } from "@/context/user";
import { loginFunc } from "@/controllers/UserController";
import { ValidateEmail } from "@/utils/utils";
import Head from "next/head";
import { useRouter } from "next/router";
import { FormEvent, useState } from "react"

export default function Login() {

    const [email, setEmail] = useState({ error: false, val: "" });
    const [password, setPassword] = useState({ error: false, val: "" });
    const [succ, setSucc] = useState({ val: 0, msg: "" })

    const { setUser } = useUser();
    const router = useRouter();
    async function signup(w: FormEvent) {
        w.preventDefault(); // prevent page from refreshing
        let errors = false;
        if (!ValidateEmail(email.val)) {
            setEmail(curr => ({ ...curr, error: true }));
            setSucc({val:2, msg: "El. paštas blogo formato"})
            errors = true;
        }
        console.log(password.val.length);
        if (password.val.trim().length === 0) {
            console.log(password.val.trim().length);
            setPassword(curr => ({ ...curr, error: true }));
            setSucc({val:2, msg: "Slaptazodis neįvestas"})
            errors = true;
        }

        if (!errors) {
            setEmail(curr => ({ ...curr, error: false }));
            setPassword(curr => ({ ...curr, error: false }));
            await loginFunc(email.val, password.val, setUser, router, setSucc);
        }
    }

    return (
        <div className="h-full bg-gray-100 py-6 flex flex-col justify-center sm:py-12">
            <Head>
                <title>Login</title>
                <meta name="description" content="Renting system" />
                <meta name="viewport" content="width=device-width, initial-scale=1" />
                <link rel="icon" href="/favicon.ico" />
            </Head>
            <div className="relative py-3 sm:max-w-xl sm:mx-auto">
                <div
                    className="absolute inset-0 bg-gradient-to-r from-blue-300 to-blue-600 shadow-lg transform -skew-y-6 sm:skew-y-0 sm:-rotate-6 sm:rounded-3xl">
                </div>
                <div className="relative px-4 py-10 bg-white shadow-lg sm:rounded-3xl sm:p-20 min-w-[500px]">
                    <div className="max-w-md mx-auto">
                        <div>
                            <h1 className="text-2xl font-semibold">Prisijungimas</h1>
                        </div>
                        <div className="divide-y divide-gray-200">
                            <form onSubmit={w => signup(w)} className="py-8 text-base leading-6 space-y-4 text-gray-700 sm:text-lg sm:leading-7">
                                <div className="relative">
                                    <input onChange={w => setEmail(curr => ({ ...curr, val: w.target.value }))} value={email.val} autoComplete="off" id="email" name="email" type="text" className={`peer placeholder-transparent h-10 w-full border-b-2 ${email.error ? 'border-red-500' : 'border-gray-300'} text-gray-900 focus:outline-none focus:borer-rose-600`} placeholder="El. paštas" />
                                    <label htmlFor="email" className="absolute left-0 -top-3.5 text-gray-600 text-sm peer-placeholder-shown:text-base peer-placeholder-shown:text-gray-440 peer-placeholder-shown:top-2 transition-all peer-focus:-top-3.5 peer-focus:text-gray-600 peer-focus:text-sm">El. paštas</label>
                                </div>
                                <div className="relative">
                                    <input onChange={w => setPassword(curr => ({ ...curr, val: w.target.value }))} value={password.val} autoComplete="off" id="password" name="password" type="password" className={`peer placeholder-transparent h-10 w-full border-b-2 ${password.error ? 'border-red-500' : 'border-gray-300'} text-gray-900 focus:outline-none focus:borer-rose-600`} placeholder="Slatažodis" />
                                    <label htmlFor="password" className="absolute left-0 -top-3.5 text-gray-600 text-sm peer-placeholder-shown:text-base peer-placeholder-shown:text-gray-440 peer-placeholder-shown:top-2 transition-all peer-focus:-top-3.5 peer-focus:text-gray-600 peer-focus:text-sm">Slaptažodis</label>
                                </div>
                                <div className="relative">
                                    <button className="bg-blue-500 text-white rounded-md px-2 py-1">Prisijungti</button>
                                </div>
                            </form>
                        </div>
                        {succ.val === 2 ? <p className="text-red-600">{succ.msg}</p> : ""}
                    </div>
                </div>
            </div>
        </div>
    )
}
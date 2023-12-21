import Link from "next/link";
import Image from 'next/image';
import { useUser } from "@/context/user";
import { useRouter } from "next/router";
import { getRole } from "@/pages/account";

export default function Navbar() {
    const { loadingState, user, setUser } = useUser();
    const router = useRouter();
    function logout() {
        setUser(null);
        router.push('/login');
    }
    console.log(getRole(user))
    return (
        <nav className="bg-gray-100">
            <div className="max-w-6xl mx-auto px-4">
                <div className="flex justify-between">

                    <div className="flex space-x-4">
                        {/* logo */}
                        <Link href="/" className="flex gap-2 items-center py-5 px-2 text-gray-700 hover:text-gray-900">
                            <Image
                                priority
                                src="/logo.svg"
                                height={32}
                                width={32}
                                alt="logo"
                            />

                            <span className="font-bold text-lg">Rentingo</span>

                        </Link>

                        {/* <!-- primary nav --> */}
                        <div className="hidden md:flex items-center space-x-1">
                            {loadingState === 'loggedin' ? <>
                                <Link href="/account" className="py-5 px-3 text-gray-700 hover:text-gray-900">Profilis</Link>
                                {getRole(user) === 'nuomotojas' ? <Link href="/newpost" className="py-5 px-3 text-gray-700 hover:text-gray-900">Naujas</Link> : ""}
                                {getRole(user) === 'nuomininkas' ? <Link href="/reservations" className="py-5 px-3 text-gray-700 hover:text-gray-900">Rezervacijos</Link> : ""}
                                {getRole(user) === 'administratorius' ? <Link className="py-5 px-3 text-gray-700 hover:text-gray-900" href={"/users"}>Vartotojai</Link> : ""}
                                <Link className="py-5 px-3 text-gray-700 hover:text-gray-900" href={"/contracts"}>Kontraktai</Link>
                            </> : ''}
                        </div>
                    </div>

                    {/* <!-- secondary nav --> */}
                    <div className="hidden md:flex items-center space-x-1">
                        {loadingState === 'loggedin' ?
                            <button onClick={() => logout()} className="py-2 px-3 bg-yellow-400 hover:bg-yellow-300 text-yellow-900 hover:text-yellow-800 rounded transition duration-300">
                                Atsijungti
                            </button> :
                            <>
                                <Link href="/login" className="py-5 px-3">Prisijungti</Link>
                                <Link href="/signup" className="py-2 px-3 bg-yellow-400 hover:bg-yellow-300 text-yellow-900 hover:text-yellow-800 rounded transition duration-300">Registruotis</Link>
                            </>}
                    </div>

                    {/* <!-- mobile button goes here --> */}
                    <div className="md:hidden flex items-center">
                        <button className="mobile-menu-button">
                            <svg className="w-6 h-6" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M4 6h16M4 12h16M4 18h16" />
                            </svg>
                        </button>
                    </div>

                </div>
            </div>
        </nav>

    )
}
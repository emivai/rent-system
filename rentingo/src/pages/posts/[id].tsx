import { IPost, Item } from "@/components/Post";
import Link from "next/link";
import Image from "next/image";
import Head from "next/head";
import { deliveryTypes } from "@/components/PostForm";
import { fetchPost } from "@/controllers/AdvertController";
import { useEffect, useState } from "react";
import { DateToISO, GetDays } from "@/utils/utils";
import { reserveItem } from "@/controllers/ReservationController";
import { useUser } from "@/context/user";
import { postContract } from "@/controllers/ContractController";
import { getRole } from "../account";
interface Props {
    post: IPost
}


export default function Post({ post }: Props) {
    const [selItemInd, setSelItemInd] = useState<number>(0);
    const [fromDate, setFromDate] = useState<string>(DateToISO(new Date()).split("T")[0]);
    const [toDate, setToDate] = useState<string>(DateToISO(new Date(new Date().getTime() + 1000 * 60 * 60 * 24)).split("T")[0])
    const [pric, setPric] = useState(0);
    const { loadingState, user } = useUser();
    const [resMsg, setResMsg] = useState("");

    const [timeLength, setTimeLength] = useState("1");
    const [contractSucc, setContractSucc] = useState({ msg: "", succ: true });
    useEffect(() => {
        try {
            const fDate = new Date(fromDate);
            const tDate = new Date(toDate);
            if (fDate.getTime() > tDate.getTime()) {
                setToDate(DateToISO(new Date(fDate.getTime() + 1000 * 60 * 60 * 24)).split("T")[0]);
                return;
            }
            if (post.items.length > 0) {
                const newPrice = GetDays(new Date(fromDate), new Date(toDate)) * parseFloat(post.items[selItemInd].price);
                setPric(newPrice);
            }
        } catch (rr) {
            console.log(rr);
        }
    }, [selItemInd, fromDate, toDate]);

    async function reserveItm() {
        const rs = await reserveItem(post.items[selItemInd], pric, fromDate, toDate, user);
        if (!rs.success) {
            setResMsg(rs.msg);
        } else {
            setResMsg("Rezervuota sėkmingai");
        }
    }

    async function rentItm() {
        const rs = await postContract(timeLength, post.items[selItemInd], user);
        setContractSucc(rs);
    }

    function getContractPrice(tm: string, item: Item) {
        try {
            const tmLen = parseInt(tm);
            if (isNaN(tmLen) || tmLen < 1) {
                setTimeLength("1");
            }
            const tprc = parseFloat(item.price);
            return Math.round(tmLen * tprc * 100) / 100;
        } catch (rr) {
            return "Netinkamas laiko arba kainos formatas";
        }
    }

    return (
        <>
            <Head>
                <title>{post.title}</title>
            </Head>
            <div>
                <div className="max-w-4xl w-full m-auto mt-2 gap-2 px-2 flex">
                    <img width={500} height={500} alt="advert logo" src={post.imageUrl ? post.imageUrl : `https://static.vecteezy.com/system/resources/thumbnails/004/141/669/small/no-photo-or-blank-image-icon-loading-images-or-missing-image-mark-image-not-available-or-image-coming-soon-sign-simple-nature-silhouette-in-frame-isolated-illustration-vector.jpg`} />
                    <div>
                        <h3 className="text-xl font-semibold text-green-600">{post.title}</h3>
                        <p className="text-gray-500">{post.description}</p>
                        <p className="text-gray-500">{deliveryTypes[post.deliveryType]}</p>
                    </div>
                </div>
                {post.items.length > 0 ? <div className="grid justify-center gap-2 my-4">
                    <select defaultValue={selItemInd} onChange={w => setSelItemInd(parseInt(w.target.value))} name="items" id="items">
                        {post.items.map((item, ind) => <option key={`itma${ind}`} value={ind}>
                            {`${item.name}, kaina: ${item.price} €/diena`}
                        </option>)}
                    </select>
                    <div className="grid justify-center gap-2 p-4 border border-gray-600 rounded-xl">
                        <p className="text-center">Rezervacija</p>
                        <div className="flex gap-2">
                            <p>Nuo:</p>
                            <input className="flex-1 border border-gray-600 rounded-lg" value={fromDate} onChange={w => setFromDate(w.target.value)} type="date" name="nuo" id="nuo" />
                        </div>
                        <div className="flex gap-2">
                            <p>Iki:</p>
                            <input className="flex-1 border border-gray-600 rounded-lg" value={toDate} onChange={w => setToDate(w.target.value)} type="date" name="iki" id="iki" />
                        </div>
                        <p className="font-bold">Kaina: {pric} €</p>
                        {loadingState === 'loggedin' && getRole(user) === 'nuomininkas' ? <>
                            <button className="bg-blue-500 text-white rounded-lg" onClick={() => reserveItm()}>Rezervuoti</button>
                            {resMsg.length > 0 ? <p>{resMsg}</p> : ""}
                        </> : "Turite buti prisijungęs ir nuomininkas"}
                    </div>
                    <div className="grid justify-center gap-2 my-4 p-4 border border-gray-600 rounded-xl">
                        <p className="text-center">Nuomojimas</p>
                        <div className="flex gap-2 ">
                            <p>Dienų kiekis: </p>
                            <input className="flex-1 border border-gray-600 rounded-lg" type="number" onChange={w => setTimeLength(w.target.value)} value={timeLength} />
                        </div>
                        <p className="font-bold">Kaina: {getContractPrice(timeLength, post.items[selItemInd])} €</p>
                        {loadingState === 'loggedin' && getRole(user) === 'nuomininkas' ? <button className="bg-blue-500 text-white rounded-lg" onClick={() => rentItm()}>Nuomoti</button> : <p>Turite buti prisijungęs ir nuomininkas</p>}
                        {contractSucc.msg.length > 0 ? <p className={`${contractSucc.succ ? 'text-green-600' : 'text-red-500'} text-center`}>{contractSucc.msg}</p> : ""}
                    </div>
                </div> : ""}
            </div>
        </>
    )
}

// Function gets called at build time on server-side.  
// It may be called again, on a serverless function, if
// revalidation is enabled and a new request comes in
export async function getStaticProps({ params }: any) {
    const post = await fetchPost(params.id);
    return {
        props: {
            post,
        },
        // Next.js will attempt to re-generate the page:
        // - When a request comes in
        // - At most once every 60 seconds
        revalidate: 10, // In seconds
    }
}

// This function gets called at build time on server-side.
// It may be called again, on a serverless function, if
// the path has not been generated.
export async function getStaticPaths() {
    const res = await fetch(`${process.env.NEXT_PUBLIC_API}/api/adverts`)
    const posts = await res.json()

    // Get the paths we want to pre-render based on posts
    const paths = posts.map((post: IPost) => ({
        params: { id: post.id.toString() },
    }))

    // We'll pre-render only these paths at build time.
    // { fallback: 'blocking' } will server-render pages
    // on-demand if the path doesn't exist.
    return { paths, fallback: 'blocking' }
}

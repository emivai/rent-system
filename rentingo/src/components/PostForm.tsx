import { ChangeEvent, FormEvent, FormEventHandler, useState } from "react"
import { Categories, IPost, Item } from "./Post"
import Image from "next/image"
import Spinner from "./Spinner"
import { DateToISO } from "@/utils/utils"

interface Props {
    onSubmit(newPost: IPost): Promise<{ msg: string, success: boolean }>,
    post: IPost
}

export const deliveryTypes = [
    "Atvažiuos savininkas",
    "Atvažiuos nuomininkas"
]

export default function PostForm({ onSubmit, post }: Props) {
    const [tpost, setTPost] = useState(post)
    const [errors, setErrors] = useState<string[]>([])
    const [emsg, setEMsg] = useState<string[]>([])
    const [file, setFile] = useState<File>();
    const [isLoading, setIsLoading] = useState(false);
    const [finished, setFinished] = useState(0);

    const handleFileChange = (e: ChangeEvent<HTMLInputElement>) => {
        if (e.target.files) {
            setFile(e.target.files[0]);
        }
    };

    async function check() {
        if (isLoading) return;
        setIsLoading(true);
        let errs = [];
        setFinished(0);
        setEMsg([]);
        if (tpost.title.length < 3) {
            errs.push("title");
            setEMsg(w => [...w, "Nėra pavadinimo"]);
        }
        if (tpost.description.length === 0) {
            setEMsg(w => [...w, "Nėra aprašymo"]);
            errs.push("desc");
        }
        if (new Date(tpost.rentStart).getTime() + 24 * 60 * 60 * 1000 >= new Date(tpost.rentEnd).getTime()) {
            setEMsg(w => [...w, "Pradžios data turi prasidėti ankščiau negu pabaigos"]);
            errs.push("date");
        }
        if (new Date(tpost.rentEnd).getTime() < new Date().getTime() + 12 * 60 * 60 * 1000) {
            setEMsg(w => [...w, "Pabaigos data pasibaigia per anksti min 12h"]);
            errs.push("date");
        }
        setErrors(errs);
        if (errs.length > 0) {
            setIsLoading(false);
            setFinished(1);
            return;
        }
        let fid: string = tpost.imageUrl ? tpost.imageUrl : "";

        if (file) {
            if (file.size > 8 * 1024 ** 2) {
                alert(`Failas per didelis max 8MB`)
                errs.push("file");
            } else {
                let fd = new FormData();
                fd.append("file", file);
                try {
                    const rs = await fetch(`https://spangle-curly-seaplane.glitch.me/api/upload`, {
                        method: "POST",
                        body: fd
                    });
                    const json = await rs.json();
                    fid = `https://cdn.discordapp.com/attachments/1025526944776867952/${json.fileid}/blob`;
                } catch (err) {
                    console.log(err);
                }
            }
        }
        try {
            const res = await onSubmit({ ...tpost, imageUrl: fid });
            if (res.success) {
                setFinished(2);
            } else {
                setFinished(1);
            }
            console.log(res.msg);
        } catch (err) {
            console.log(err);
            setFinished(1);
        }
        setIsLoading(false)
    }

    function addMoreItems() {
        setTPost(curr => ({ ...curr, items: [...curr.items, { name: "", price: "0", category: 1, state: 1 }] }));
    }

    return (
        <div className="py-8 text-base leading-6 grid gap-2 text-gray-700 sm:text-lg sm:leading-7">
            <div className="relative">
                <input onChange={w => setTPost(curr => ({ ...curr, title: w.target.value }))} value={tpost.title} autoComplete="off" id="title" name="title" type="text" className={`peer placeholder-transparent h-10 w-full border-b-2 ${errors.includes("title") ? 'border-red-500' : 'border-gray-300'} text-gray-900 focus:outline-none focus:borer-rose-600`} placeholder="Pavadinimas" />
                <label htmlFor="title" className="absolute left-0 -top-3.5 text-gray-600 text-sm peer-placeholder-shown:text-base peer-placeholder-shown:text-gray-440 peer-placeholder-shown:top-2 transition-all peer-focus:-top-3.5 peer-focus:text-gray-600 peer-focus:text-sm">Pavadinimas</label>
            </div>
            <div className="relative">
                <p className="text-gray-600 text-base">Apibendrinimas</p>
                <textarea onChange={w => setTPost(curr => ({ ...curr, description: w.target.value }))} value={tpost.description} name="desc" id="desc" cols={30} rows={10} placeholder="Description" className={`peer placeholder-transparent h-10 w-full border rounded-lg p-1 ${errors.includes("desc") ? 'border-red-500' : 'border-gray-300'} text-gray-900 focus:outline-none focus:borer-rose-600`}></textarea>
            </div>
            <div>
                <p className="text-gray-600 text-base">Nuotrauka:</p>
                <input type="file" name="file" id="file" onChange={handleFileChange} accept="image/png, image/jpeg" />
                {tpost.imageUrl ? <div className="mt-2">
                    <p className="text-gray-600 text-base">Dabartinė nuotrauka</p>
                    <img width={400} src={tpost.imageUrl} />
                </div> : ""}
            </div>
            <div>
                <p className="text-gray-600 text-base">Pristatymo tipas:</p>
                <select onChange={w => setTPost(curr => ({ ...curr, deliveryType: parseInt(w.target.value) }))} className="w-1/2" name="delivery" id="delivery">
                    {deliveryTypes.map((w, ind) => (
                        <option key={ind} value={ind}>{w}</option>
                    ))}
                </select>
            </div>
            <div>
                <p className="text-gray-600 text-base">Pradžios data:</p>
                <input type="datetime-local" className={`border rounded-lg p-1 ${errors.includes("date") ? "border-red-500" : "border-gray-400"}`} onChange={w => setTPost(curr => ({ ...curr, rentStart: new Date(w.target.value) }))} value={DateToISO(tpost.rentStart)} name="startd" id="startd" />
            </div>
            <div>
                <p className="text-gray-600 text-base">Pabaigos data:</p>
                <input type="datetime-local" className={`border rounded-lg p-1 ${errors.includes("date") ? "border-red-500" : "border-gray-400"}`} onChange={w => setTPost(curr => ({ ...curr, rentEnd: new Date(w.target.value) }))} value={DateToISO(tpost.rentEnd)} name="endd" id="endd" />
            </div>
            {tpost.items.length > 0 ? <div className="grid gap-3 justify-center w-full mt-3">
                <h2>Daiktai</h2>
                {tpost.items.map((item, ind) => <div key={`itm${ind}`} className="flex gap-4">
                    <div className="relative">
                        <input onChange={w => setTPost(curr => { curr.items[ind].name = w.target.value; return { ...curr }; })} value={item.name} autoComplete="off" id="title2" name="title2" type="text" className={`peer placeholder-transparent h-10 w-full border-b-2 text-gray-900 focus:outline-none focus:borer-rose-600`} placeholder="Pavadinimas" />
                        <label htmlFor="title2" className="absolute left-0 -top-3.5 text-gray-600 text-sm peer-placeholder-shown:text-base peer-placeholder-shown:text-gray-440 peer-placeholder-shown:top-2 transition-all peer-focus:-top-3.5 peer-focus:text-gray-600 peer-focus:text-sm">Pavadinimas</label>
                    </div>
                    <div className="relative">
                        <p className="absolute text-xs top-[-14px] left-[5px]">Kategorija</p>
                        <select onChange={w => setTPost(curr => { curr.items[ind].category = parseInt(w.target.value); return { ...curr } })}  name="cate" id="cate">
                            {Categories.map((w, ind2) => <option key={`opt${ind}${ind2}`} value={ind2}>
                                {w}
                            </option>)}
                        </select>
                    </div>
                    <div className="relative">
                        <input onChange={w => setTPost(curr => { curr.items[ind].price = w.target.value; return { ...curr } })} value={item.price} autoComplete="off" id="title4" name="title4" min="1" step="any" type="number" className={`peer placeholder-transparent h-10 w-full border-b-2 text-gray-900 focus:outline-none focus:borer-rose-600`} placeholder="Kaina" />
                        <label htmlFor="title4" className="absolute left-0 -top-3.5 text-gray-600 text-sm peer-placeholder-shown:text-base peer-placeholder-shown:text-gray-440 peer-placeholder-shown:top-2 transition-all peer-focus:-top-3.5 peer-focus:text-gray-600 peer-focus:text-sm">Kaina</label>
                    </div>
                    <button className="bg-blue-500 text-white rounded-md px-2 py-1" onClick={() => setTPost(curr => ({ ...curr, items: [...curr.items.slice(0, ind), ...curr.items.slice(ind + 1)] }))}>Naikinti</button>
                </div>)}
            </div> : ""
            }
            <button onClick={() => addMoreItems()} className="bg-blue-500 text-white rounded-md px-2 py-1">Naujas daiktas</button>
            <div className="relative">
                {isLoading ?
                    <Spinner /> :
                    <button onClick={() => check()} className="bg-blue-500 text-white rounded-md px-2 py-1">Pateikti</button>}
            </div>
            {finished === 1 ? <p className="text-red-500">Nepavyko</p> : ""}
            {finished === 2 ? <p className="text-green-500">Pavyko</p> : ""}
            {
                emsg.length > 0 ? <div>{emsg.map((w: string, ind: number) => (
                    <p key={`err-${ind}`} className="text-red-500">{w}</p>
                ))}</div> : ""
            }
        </div >
    )
}
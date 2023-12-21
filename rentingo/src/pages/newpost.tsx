import { IPost } from "@/components/Post";
import PostForm from "@/components/PostForm";
import Spinner from "@/components/Spinner";
import { useUser } from "@/context/user";
import { createPost } from "@/controllers/AdvertController";
import Head from "next/head";
import { useRouter } from "next/router";
import { useEffect, useState } from "react";

export default function NewPost() {
    const [post, setPost] = useState<IPost>();
    const { loadingState, user } = useUser();
    const router = useRouter();
    useEffect(() => {
        if (loadingState === 'loading') return;
        if (loadingState === 'loggedout' || !user) router.push("/login");
        else {
            setPost({
                description: "",
                videoUrl: "",
                deliveryType: 1,
                rentStart: new Date(),
                rentEnd: new Date(new Date().getTime() + Math.round(1000 * 60 * 60 * 24 * 10 * (Math.random() / 2 + 0.5))),
                id: 0,
                title: "",
                user: user.profile,
                items: []
            })
        }
    }, [loadingState]);

    return (
        <div className="mx-auto w-1/2 mt-2">
            <Head>
                <title>Naujas skelbimas</title>
                <meta name="description" content="Renting system" />
                <meta name="viewport" content="width=device-width, initial-scale=1" />
                <link rel="icon" href="/favicon.ico" />
            </Head>
            {loadingState === 'loggedin' && post ?
                <PostForm onSubmit={w => createPost(w, user)} post={post} /> : <div className="flex justify-center"><Spinner /></div>}
        </div>
    )
}
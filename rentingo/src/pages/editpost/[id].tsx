import { IPost } from "@/components/Post";
import Link from "next/link";
import Head from "next/head";
import { useEffect, useState } from "react";
import PostForm from "@/components/PostForm";
import { useUser } from "@/context/user";
import { useRouter } from "next/router";
import Spinner from "@/components/Spinner";
import { editPost, fetchPost } from "@/controllers/AdvertController";

export default function Post() {
    const [post, setPost] = useState<IPost>()
    const { loadingState, user } = useUser();
    const router = useRouter();
    const { id } = router.query;
    useEffect(() => {
        async function fetchPostFunc() {
            if(id){
                const post = await fetchPost(id as string)
                setPost(post);
            }
        }
        if (loadingState === 'loading') return;
        if (loadingState === 'loggedout') router.push('/login');
        else {
            fetchPostFunc();
        }
    }, [loadingState, id])

    return (
        <div className="mx-auto w-1/2 mt-2">
            <Head>
                <title>{post ? post.title : 'Kraunama'}</title>
            </Head>
            {post ? <PostForm onSubmit={w => editPost(w, user)} post={post} /> : <div className="flex justify-center"><Spinner /></div>}
        </div>
    )
}
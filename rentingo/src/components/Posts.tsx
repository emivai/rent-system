import { LoadingState, User } from "@/context/user";
import Post, { IPost } from "./Post";

interface Props {
    posts: IPost[],
    setPosts: Function,
    user: User | null,
    loadingState?: LoadingState
}

export default function Posts({ posts, setPosts, user, loadingState }: Props) {

    async function delPost(post: IPost) {
        if (loadingState === 'loading' || loadingState === 'loggedout' || !user) return;
        if (confirm(`Ar tikrai norite ištrinti ${post.title}?`)) {
            try {
                await fetch(`${process.env.NEXT_PUBLIC_API}/api/adverts?id=${post.id}`, { method: "DELETE", headers: { 'Authorization': `Bearer ${user.token}` } })
                setPosts((curr: IPost[]) => [...curr.filter((a: IPost) => a.id !== post.id)])
            } catch (err: any) {
                console.log(err);
                alert("failed to delete " + err.message);
            }
        }
    }

    return (
        <div className="max-w-4xl w-full m-auto px-2 grid">
            {posts.map((post, ind) => <Post user={user} loadingState={loadingState} key={ind} post={post} delPost={delPost} />)}
        </div>
    )
}
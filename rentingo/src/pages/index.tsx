import Post, { IPost } from '@/components/Post'
import { Categories } from '@/components/Post';
import Posts from '@/components/Posts';
import Spinner from '@/components/Spinner';
import { useUser } from '@/context/user';
import { fetchPosts } from '@/controllers/AdvertController';
import Head from 'next/head'
import { useEffect, useState } from 'react'

export default function Home() {
  const [posts, setPosts] = useState<IPost[]>([])
  const [isLoading, setIsLoading] = useState(true);
  const { loadingState, user } = useUser();

  async function refetchPosts(category?: number) {
    setPosts(await fetchPosts(category));
  }

  useEffect(() => {
    async function getPosts() {
      try {
        await refetchPosts();
      } catch (rr) {
        console.log(rr);
      } finally {
        setIsLoading(false);
      }
    }
    getPosts();
  }, [loadingState])

  return (
    <>
      <Head>
        <title>Rentingo</title>
        <meta name="description" content="Renting system" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <link rel="icon" href="/favicon.ico" />
      </Head>
      {isLoading ? <div className='flex justify-center mt-2'>
        <Spinner />
      </div> :
        <div className='grid'>
          <div className='grid my-2 gap-2 justify-center'>
            <p>Kategorija</p>
            <select defaultValue={-1} onChange={w => refetchPosts(parseInt(w.target.value))} name="categ" id="categ">
              <option value={-1}>Visos</option>
              {Categories.map((w, ind) => <option key={`${ind}catgry`} value={ind}>{w}</option>)}
            </select>
          </div>
          {posts.length > 0 ? <>
            <Posts loadingState={loadingState} user={user} posts={posts} setPosts={setPosts} />
          </> : <p>Skelbimų nėra</p>}
        </div>}
    </>
  )
}

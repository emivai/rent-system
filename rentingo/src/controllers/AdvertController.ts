import { IPost } from "@/components/Post";
import { User } from "@/context/user";

export async function fetchPost(id: string) {
    const res = await fetch(`${process.env.NEXT_PUBLIC_API}/api/adverts/${id}`);
    const post = await res.json();
    return post;
}

export async function fetchPosts(category?: number) {
    const rs = await fetch(`${process.env.NEXT_PUBLIC_API}/api/adverts${category ? `?category=${category}` : ''}`);
    const posts = await rs.json();
    return posts;
}

export async function editPost(newPost: IPost, user: User | null) {
    const id = newPost.id;
    let startDate = new Date(new Date().getTime() + 1000 * 60 * 60 * 24)
    let endDate = new Date(new Date().getTime() + 1000 * 60 * 60 * 24 * 10)
    let obj: any = { ...newPost, rentStart: startDate, rentEnd: endDate }
    delete obj.id;
    const rs = await fetch(`${process.env.NEXT_PUBLIC_API}/api/adverts/${id}`, {
        method: "PUT",
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${user?.token}`
        },
        body: JSON.stringify(obj)
    })
    if (rs.ok) {
        return { success: true, msg: 'Sėkmingai išsaugota' };
    }
    return { success: false, msg: 'Nepavyko išsaugoti' };
}

export async function createPost(newPost: IPost, user: User | null) {
    let obj: any = {
        title: newPost.title,
        description: newPost.description,
        imageUrl: newPost.imageUrl,
        deliveryType: newPost.deliveryType,
        rentStart: newPost.rentStart,
        rentEnd: newPost.rentEnd
    }
    const rs = await fetch(`${process.env.NEXT_PUBLIC_API}/api/adverts`, {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${user?.token}`
        },
        body: JSON.stringify(obj)
    });
    const rs2 = await fetch(`${process.env.NEXT_PUBLIC_API}/api/adverts`);
    const posts = await rs2.json();
    const insertedId = posts[posts.length - 1].id;
    if (rs.ok) {
        for (let i = 0; i < newPost.items.length; i++) {
            const itmObj = {
                name: newPost.items[i].name,
                category: newPost.items[i].category,
                price: newPost.items[i].price,
                state: newPost.items[i].state,
                advertId: insertedId
            };
            const rs3 = await fetch(`${process.env.NEXT_PUBLIC_API}/api/items`, {
                method: "POST",
                headers: {
                    "Content-Type": 'application/json',
                    "Authorization": `Bearer ${user?.token}`
                },
                body: JSON.stringify(itmObj)
            })
        }
        return { msg: "Pavyko", success: true };
    }
    return { msg: "Nepavyko", success: false };
}
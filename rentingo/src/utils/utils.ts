export function ValidateEmail(mail: string) {
    if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(mail)) {
        return true
    }
    return false
}

export function GetDays(from: Date, to: Date) {
    const diff = to.getTime() - from.getTime();
    return Math.round(diff / (1000 * 60 * 60 * 24));
}

export function DateToISO(date: Date) {
    let dt = null;
    if (typeof date === "string") {
        try {
            date = new Date(date);
        } catch (err) {
            console.log("failed to convert string to date")
            console.log(err);
        }
    }
    if (typeof date === 'object' && date !== null && 'toISOString' in date) {
        dt = date;
    } else dt = new Date();
    const r = dt.toISOString().split('.');
    return r[0];
}
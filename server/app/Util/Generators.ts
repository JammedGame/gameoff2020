export const generateId = () => {
    let DT: number = new Date().getTime();
    let Uuid: string = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, (C) => {
        let R: number = (DT + Math.random() * 16) % 16 | 0;
        DT = Math.floor(DT / 16);
        return (C == 'x' ? R : (R & 0x3 | 0x8)).toString(16);
    });
    return Uuid;
}

namespace BlazingBlog.Models
{
    public record struct LoggedInUser(int UserId, string DisplayName)
    {
        public readonly bool IsEmpty => UserId == 0; //Is empty คือเมื่อ user id เป็น 0
    }
} 
//ตัวแปรที่ประกาศไว้เป็น Class ต้องทำการ New จึงจะเป็นตัวขึ้นมา(ในหน่วยความจำ) แต่ Structure ประกาศปุ๊บมีตัวของมันขึ้นมาทันที 
//ถ้าจะเรียกให้ถูกคือ Instance หรือ Object ของคลาสจะถูกสร้างขึ้นใน Heap แต่ค่าของ Structure จะถูกสร้างขึ้นใน Stack
//กรณี Class นั้นอาจมีตัวแปรสองตัวที่ชี้มายัง Object เดียวกันได้ แต่กรณี Structure นั้น ตัวแปรหนึ่งตัว ก็คือหนึ่งตำแหน่งในหน่วยความจำแน่นอน


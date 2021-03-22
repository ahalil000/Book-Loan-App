export class BookLoanStatus
{
    public id: number;
    public status: string; 
    public dateLoaned: string;
    public dateDue: string; 
    public dateReturn: string;
    public borrower: string;
    public onShelf: boolean;
}

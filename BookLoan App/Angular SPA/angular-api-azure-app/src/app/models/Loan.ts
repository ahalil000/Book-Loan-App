export class Loan
{
    public id: number;
    public bookId: number;
    public dateLoaned: string;
    public daysLoaned: number;
    public dateDue: string;
    public dateReturned?: string;
    public borrowerMemberShipNo: string;
    public loanedBy: string;
}
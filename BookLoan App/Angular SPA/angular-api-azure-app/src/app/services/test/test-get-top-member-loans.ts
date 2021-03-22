import { TopMemberLoansReport } from 'src/app/models/TopMemberLoansReport';
import { getUsers } from './test-users';

export function getTopMemberLoans(): TopMemberLoansReport[] {
    var member1 = getUsers().find(u => u.id === '1');
    var member2 = getUsers().find(u => u.id === '2');
    var member3 = getUsers().find(u => u.id === '3');
    return [
      { ranking: 1, count: 3, userName: member1.userName, userEmail: member1.email },
      { ranking: 2, count: 2, userName: member2.userName, userEmail: member2.email },
      { ranking: 3, count: 1, userName: member3.userName, userEmail: member3.email }
    ];
}

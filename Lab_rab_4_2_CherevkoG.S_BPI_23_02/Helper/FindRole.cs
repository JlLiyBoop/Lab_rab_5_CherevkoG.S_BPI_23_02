using Lab_rab_4_2_CherevkoG.S_BPI_23_02.Model;

namespace Lab_rab_4_2_CherevkoG.S_BPI_23_02.Helper
{
    public class FindRole
    {
        int id;
        public FindRole(int id) { this.id = id; }
        public bool RolePredicate(Role role) => role.Id == id;
    }
}
using System;
namespace dsapi.Models
{
	public class IdRoleModel
	{
		public IdRoleModel(int id)
		{
			this.Id = id;
		}

		public IdRoleModel(int id, string roleName)
		{
			this.Id = id;
			this.RoleName = roleName;
		}

		public int Id { get; set; }
		public string RoleName { get; set; }
	}
}


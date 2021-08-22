using System.Collections.Generic;

namespace TFS_Parser
{
	public class WorkOperations
	{

	}

	public struct WorkAlternativ
	{
		int m_nID;
		string m_sName;
		double m_dB, m_dT, m_dV;
	};

	public struct CheckAlternativ
	{
		int m_nID;
		string m_sName;
		double m_dK00, m_dK11, m_dTf, m_dVf;
	};

	public class BasisOperation
	{
		int m_nType;

		public List<BasisOperation> m_ListOperationIn;
		public List<BasisOperation> m_ListOperationBefore;

		public BasisOperation(int nType)
		{
			m_nType = nType;
			m_ListOperationIn = new List<BasisOperation>();
		}

		//virtual void PutOnWork(TMakerTFS* Maker) {};
		//virtual void PutOnAlter(TMakerTFS* Maker, int nId) {};
	};

	class CheckOperation : BasisOperation
	{
		int m_nID;
		List<BasisOperation> m_ListCheckWork;
		List<BasisOperation> m_ListCheckAlter;

		public CheckOperation(int nType) : base(2)
		{
		}

		//void PutOnWork(TMakerTFS* Maker);
		//void PutOnAlter(TMakerTFS* Maker, int nId);
	};

	class WorkOperation : BasisOperation
	{

		int m_nID;
		List<int> m_ListWorkAlter;

		CheckOperation m_pCheckAlone;
		CheckOperation m_pGroupCheck;


		public WorkOperation(int nType) : base(1)
		{
		}

		//void PutOnWork(TMakerTFS* Maker);
		//void PutOnAlter(TMakerTFS* Maker, int nId);
	};

	class ParallWorkOperation : BasisOperation
	{
		BasisOperation m_op1, m_op2;
		bool m_bParal;

		ParallWorkOperation(BasisOperation op1, BasisOperation op2, bool bParal) : base(3)
		{
			m_op1 = op1;
			m_op2 = op2;
			m_bParal = bParal;
			m_ListOperationBefore = new List<BasisOperation>();

			for (int i = 0; i < op1.m_ListOperationBefore.Count; i++)
			{
				m_ListOperationBefore.Add(op1.m_ListOperationBefore[i]);
			}

			for (int i = 0; i < op2.m_ListOperationBefore.Count; i++)
			{
				bool bAdd = true;
				for (int j = 0; j < m_ListOperationBefore.Count; j++)
				{
					if (m_ListOperationBefore[j] == op2.m_ListOperationBefore[i])
					{
						bAdd = false;
						break;
					}
				}

				if (bAdd)
				{
					m_ListOperationBefore.Add(op2.m_ListOperationBefore[i]);
				}
			}

			for (int i = 0; i < op1.m_ListOperationIn.Count; i++)
			{
				m_ListOperationIn.Add(op1.m_ListOperationIn[i]);
			}

			for (int i = 0; i < op2.m_ListOperationIn.Count; i++)
			{
				bool bAdd = true;
				for (int j = 0; j < m_ListOperationIn.Count; j++)
				{
					if (m_ListOperationIn[j] == op2.m_ListOperationIn[i])
					{
						bAdd = false;
						break;
					}
				}

				if (bAdd)
				{
					m_ListOperationIn.Add(op2.m_ListOperationIn[i]);
				}
			}
		}

		//void PutOnWork(TMakerTFS* Maker);
		//void PutOnAlter(TMakerTFS* Maker, int nId);

	};

}
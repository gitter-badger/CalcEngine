using System.Collections.Generic;
using CalcEngine.Expressions.Functions;


namespace CalcEngine.Expressions
{
	/// <summary>
	/// Defines a Function to be executed within the Abstract Syntax Tree. This
	/// accepts parameters as child Nodes.
	/// </summary>
	internal class AstFunctionNode : AstNode
	{
		/// <summary>
		/// Name of the function
		/// </summary>
		private readonly string _name;

		/// <summary>
		/// The function class used to evaluate the node
		/// </summary>
		private readonly PostfixMathCommand _pfmc;


		/// <summary>
		/// Initializes a new instance of the AstFunctionNode class given the function name and the math command.
		/// </summary>
		/// <param name="function">The PostFixMathCommand object used to process this
		/// function's parameters and supply the result.</param>
		/// <param name="functionName">The name of the function, or operator symbol.</param>
		internal AstFunctionNode( PostfixMathCommand function, string functionName )
		{
			_pfmc = function;
			_name = functionName;
		}


		/// <summary>
		/// Initializes a new instance of the AstFunctionNode class given the function name and the math command.
		/// </summary>
		/// <param name="function">The PostFixMathCommand object used to process this
		/// function's parameters and supply the result.</param>
		/// <param name="children">The children.</param>
		internal AstFunctionNode( PostfixMathCommand function, IEnumerable< AstNode > children )
		{
			_pfmc = function;
			_name = function.Symbol;

			foreach ( AstNode child in children )
				AddChild( child );
		}


		/// <summary>
		/// Gets the name of the node (operator symbol or function name).
		/// </summary>
		internal virtual string Name
		{
			get { return _name; }
		}

		/// <summary>
		/// Gets the math command class associated with this node.
		/// </summary>
		internal virtual PostfixMathCommand Pfmc
		{
			get { return _pfmc; }
		}

		public override int Type
		{
			get
			{
				const int NodeType = 0;
				string cs0 = Pfmc.Symbol;
				if ( cs0 == null )
					return NodeType;
				if ( cs0 != "==" )
				{
					if ( cs0 != "and" )
						return NodeType;
				}
				else
					return 4;
				return 7;
			}
		}


		/// <summary>
		/// Accept a visitor object. The visitor must be of type IAstNodeVisitor, however
		/// if the visitor also implements the more type-specific IAstFunctionNodeVisitor,
		/// it uses its interface for the visit callback.
		/// <p/>
		/// This is an implementation of the AcyclicVisitor pattern by Robert C. Martin.
		/// Details of the pattern and its usage can be found at ObjectMentor.com.<br/>
		/// URL: http://www.objectmentor.com/resources/articles/acv.pdf
		/// </summary>
		/// <param name="visitor">
		/// The object visiting this node.
		/// </param>
		/// <param name="sessionData">
		/// Misc data to use during this visit.
		/// </param>
		/// <returns>
		/// The session data object.
		/// </returns>
		internal override object Accept( IAstNodeVisitor visitor, object sessionData )
		{
			if ( visitor is IAstFunctionNodeVisitor )
				return ( (IAstFunctionNodeVisitor)visitor ).Visit( this, sessionData );

			return visitor.Visit( this, sessionData );
		}


		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />.</param>
		/// <returns>
		/// <see langword="true" /> if the specified <see cref="T:System.Object" /> is equal to the
		/// current <see cref="T:System.Object" />; otherwise, <see langword="false" />.
		/// </returns>
		public override bool Equals( object obj )
		{
			if ( ( obj == null ) || ( GetType() != obj.GetType() ) )
				return false;
			var node = (AstFunctionNode)obj;
			return Name.ToLower().Equals( node.Name.ToLower() ) && ChildrenEqual( this, node );
		}


		/// <summary>
		/// Serves as a hash function for a particular type, suitable
		/// for use in hashing algorithms and data structures like a hash table.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object" />.
		/// </returns>
		public override int GetHashCode()
		{
			int result = Name.GetHashCode();
			if ( Children != null )
			{
				for ( int i = 0; i < Children.Count; i++ )
					result ^= GetChild( i ).GetHashCode();
			}

			return result;
		}


		/// <summary>
		/// Returns a string containing the function name.
		/// </summary>
		/// <returns>
		/// string containing the function name..
		/// </returns>
		public override string ToString()
		{
			string outString = "( Function \"" + Name + "\" ";
			if ( Children != null )
			{
				for ( int i = 0; i < Children.Count; i++ )
					outString = outString + GetChild( i );
			}

			return outString + ") ";
		}
	}
}
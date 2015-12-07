using System;
using System.Web;
using NHibernate;

namespace NHibernate.Helper.Management
{
    /// <summary>
    /// Implementa o padr�o Open-Session-In-View usando <see cref="SessionManager" />.
    /// Assume que cada solicita��o HTTP � dada uma �nica transa��o para todo o ciclo de vida p�gina.
    /// A inspira��o para essa classe veio de Ed Courtenay em
    /// http://sourceforge.net/forum/message.php?msg_id=2847509.
    /// </summary>
    public class SessionModule : IHttpModule
    {
        public void Init(HttpApplication context) {
            context.BeginRequest += new EventHandler(BeginTransaction);
            context.EndRequest += new EventHandler(CommitAndCloseSession);
        }

        /// <summary>
        /// Abre uma sess�o no inicio do pedido HTTP.
        /// N�o abre uma conex�o com o banco at� que seja necess�rtio. (Open Session In View)
        /// </summary>
        private void BeginTransaction(object sender, EventArgs e) {
            SessionManager.InitNHibernateSession();
        }

        /// <summary>
        /// Fecha a sess�o NHibernate fornecido pelo <see cref="SessionManager"/>.
        /// Assume que a transa��o foi inicializada pelo <see cref="TransactionManagementAspect"/>
        /// </summary>
        private void CommitAndCloseSession(object sender, EventArgs e) {
            ISession session = SessionManager.Session;
            //fechando e finalizando a sess�o do contexto
            session.Close();
            SessionManager.Session = null;
        }

        public void Dispose() { }
    }
}

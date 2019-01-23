using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb
{
    public partial class Login : BasePage
    {
        Esito esito = new Esito();
        protected void Page_Load(object sender, EventArgs e)
        {
            caricaListeTipologiche();

            if (!this.IsPostBack)
            {
                //string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
                string constr = ConfigurationManager.ConnectionStrings["constrMSSQL"].ConnectionString;
                Session["connectionString"] = constr;
                Session[SessionManager.UTENTE] = null;
            }
        }

        protected void btnLogIn_Click(object sender, EventArgs e)
        {
            lblErrorLogin.Visible = false;
            //string constr = Session["connectionString"].ToString();

            //if (Convert.ToBoolean(ConfigurationManager.AppSettings["USA_DB"]))
            //{
            //    using (MySqlConnection con = new MySqlConnection(constr))
            //    {
            //        using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM anag_utenti where username = '" + tbUser.Text.Trim() + "' and password = '" + tbPassword.Text.Trim() + "'"))
            //        {
            //            using (MySqlDataAdapter sda = new MySqlDataAdapter())
            //            {
            //                cmd.Connection = con;
            //                sda.SelectCommand = cmd;
            //                using (DataTable dt = new DataTable())
            //                {
            //                    sda.Fill(dt);
            //                    //GridView1.DataSource = dt;
            //                    //GridView1.DataBind();
            //                    if (dt != null && dt.Rows != null && dt.Rows.Count == 1)
            //                    {
            //                        //Anag_Utente utenteConnesso = new Anag_Utente();
            //                        //utenteConnesso.Nome = dt.Rows[0]["Nome"].ToString();
            //                        //utenteConnesso.Cognome = dt.Rows[0]["Cognome"].ToString();
            //                        //utenteConnesso.Descrizione = dt.Rows[0]["Descrizione"].ToString();
            //                        //utenteConnesso. = dt.Rows[0]["Nome"].ToString();

            //                        Session[SessionManager.UTENTE] = dt.Rows[0];
            //                        Response.Redirect("Agenda.aspx", true);

            //                    }
            //                } 
            //            }
            //        }
            //    }
            //}

                string constr = Session["connectionString"].ToString();
                Anag_Utente u = new Anag_Utente();
            try
            {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["USA_DB"]))
                {
                    using (System.Data.OleDb.OleDbConnection con = new System.Data.OleDb.OleDbConnection(constr))
                    {
                        using (System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand("SELECT * FROM anag_utenti where username = '" + tbUser.Text.Trim() + "' and password = '" + tbPassword.Text.Trim() + "'"))
                        {
                            using (System.Data.OleDb.OleDbDataAdapter sda = new System.Data.OleDb.OleDbDataAdapter())
                            {
                                cmd.Connection = con;
                                sda.SelectCommand = cmd;
                                using (DataTable dt = new DataTable())
                                {
                                    sda.Fill(dt);
                                    //GridView1.DataSource = dt;
                                    //GridView1.DataBind();
                                    if (dt != null && dt.Rows != null && dt.Rows.Count == 1)
                                    {
                                        //Anag_Utente utenteConnesso = new Anag_Utente();
                                        //utenteConnesso.Nome = dt.Rows[0]["Nome"].ToString();
                                        //utenteConnesso.Cognome = dt.Rows[0]["Cognome"].ToString();
                                        //utenteConnesso.Descrizione = dt.Rows[0]["Descrizione"].ToString();
                                        //utenteConnesso. = dt.Rows[0]["Nome"].ToString();


                                        
                                        u.Cognome = dt.Rows[0]["Cognome"].ToString();
                                        u.Nome = dt.Rows[0]["Nome"].ToString();
                                        u.username = dt.Rows[0]["Username"].ToString();
                                        //Session[SessionManager.UTENTE] = dt.Rows[0];
                                        Session[SessionManager.UTENTE] = u;
                                        
                                        //Server.Execute("~/grigliaNicola.aspx",true);
                                    }
                                    else
                                    {
                                        lblErrorLogin.Text = "Utenza/Password non riconosciute!";
                                        lblErrorLogin.Visible = true;
                                    }
                                }
                            }
                        }
                    }
                }

                else
                {
                    Tipologica tipoUtente = UtilityTipologiche.getElementByID(listaTipiUtente, 2, ref esito);
                    Session[SessionManager.UTENTE] = new Anag_Utente() { id = 1, Nome = "Sandro", Cognome = "Chiappa", id_tipoUtente = tipoUtente.id };
                }

            }
            catch (Exception ex)
            {
                string eccezione = "Login.aspx.cs - btnLogIn_Click " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
                Session["ErrorPageText"] = eccezione;
                string url = String.Format("~/pageError.aspx");
                Response.Redirect(url, true);
            }
            if (Session[SessionManager.UTENTE]!=null) Response.Redirect("~/Agenda/Agenda.aspx");
        }
    }
}
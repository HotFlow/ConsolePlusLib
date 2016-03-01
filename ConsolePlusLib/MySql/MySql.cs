using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlTypes;
using ConsolePlusLib.Utils.Output;

namespace ConsolePlusLib.MySql
{
    public class IMySql
    {
        /// <summary>
        /// MySql连接
        /// </summary>
        private MySqlConnection Connection { get; set; }

        /// <summary>
        /// 协议
        /// </summary>
        private String Contract { get; set; }

        /// <summary>
        /// MySql命令
        /// </summary>
        private MySqlCommand Command { get; set; }

        /// <summary>
        /// MySql数据读取
        /// </summary>
        private MySqlDataReader Reader { get; set; }

        /// <summary>
        /// 数据库
        /// </summary>
        private String Schema { get; set; }

        /// <summary>
        /// 表
        /// </summary>
        private String Table { get; set; }

        /// <summary>
        /// 是否开启
        /// </summary>
        public Boolean Enable { get; set; } 

        /// <summary>
        /// 构建MySql连接
        /// </summary>
        /// <param name="host">地址</param>
        /// <param name="port">端口</param>
        /// <param name="schema">数据库</param>
        /// <param name="table">表</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        public IMySql(String host, int port, String schema, String table, String username, String password)
        {
            this.Enable = false;
            try
            {
                this.Contract = "datasource=" + host + ";port=" + port + ";username=" + username + ";password=" + password +
                    ";Convert Zero Datetime=True;Allow Zero DateTime=True;";
                this.Schema = schema;
                this.Table = table;
                this.Connection = new MySqlConnection(this.Contract);
                this.Connection.Open();
                this.Enable = true;
                ConsolePlusLib.Console.Out.println("MySql已开启!");
                this.Connection.Close();
            }
            catch(MySqlException)
            {
                this.Enable = false;
                ConsolePlusLib.Console.Out.println("MySql信息出错,已关闭!");
            }
        }

        /// <summary>
        /// 创建表格
        /// </summary>
        /// <returns>是否成功</returns>
        public Boolean createTable(MySqlSlot[] slots)
        {
            lock (this)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < slots.Length; i++)
                    {
                        MySqlSlot slot = slots[i];
                        sb.Append(slot.flag);
                        if (i < slots.Length - 1)
                        {
                            sb.Append(", ");
                        }
                    }

                    this.Command = new MySqlCommand("CREATE TABLE " + this.Schema + "." + this.Table + "(" + sb.ToString() + ");", Connection);
                    this.Connection.Open();
                    this.Reader = this.Command.ExecuteReader();

                    while (this.Reader.Read())
                    {
                        this.closeReader();
                        this.Connection.Close();
                        return true;
                    }
                }
                catch (MySqlException ex)
                {
                    this.closeReader();
                    this.Connection.Close();
                    ConsolePlusLib.Console.Out.println(Level.Severe, ex.ToString());
                }
                return false;
            }
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>数量</returns>
        public int insert(String key, String value)
        {
            lock (this)
            {
                try
                {
                    this.Command = new MySqlCommand("Insert into " + this.Schema + "." + this.Table + " (" + key + ")" + " values (" + value + ")", Connection);
                    this.Connection.Open();
                    this.Reader = this.Command.ExecuteReader();

                    int i = 0;
                    while (this.Reader.Read())
                    {
                        i++;
                    }

                    return i;
                }
                catch (MySqlException ex)
                {
                    ConsolePlusLib.Console.Out.println(Level.Severe, ex.ToString());
                    return 0;
                }
                finally
                {
                    this.closeReader();
                    this.Connection.Close();
                }
            }
        }


        /// <summary>
        /// 删除行
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>数量</returns>
        public int delete(MySqlCondition condition)
        {
            lock (this)
            {
                try
                {
                    this.Command = new MySqlCommand("DELETE FROM " + this.Schema + "." + this.Table + " WHERE " + condition.condition + ";", Connection);
                    this.Connection.Open();
                    this.Reader = this.Command.ExecuteReader();

                    int i = 0;
                    while (this.Reader.Read())
                    {
                        i++;
                    }

                    return i;
                }
                catch (MySqlException ex)
                {
                    ConsolePlusLib.Console.Out.println(Level.Severe, ex.ToString());
                    return 0;
                }
                finally
                {
                    this.closeReader();
                    this.Connection.Close();
                }
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>数量</returns>
        public int update(MySqlCondition condition, String key, String value)
        {
            lock (this)
            {
                try
                {
                    this.Command = new MySqlCommand("Update " + this.Schema + "." + this.Table + " SET" + key + "='" + value + "' WHERE " + condition.condition + ";", this.Connection);
                    this.Connection.Open();
                    this.Reader = this.Command.ExecuteReader();

                    int i = 0;
                    while (this.Reader.Read())
                    {
                        i++;
                    }

                    return i;
                }
                catch (MySqlException ex)
                {
                    ConsolePlusLib.Console.Out.println(Level.Severe, ex.ToString());
                    return 0;
                }
                finally
                {
                    this.closeReader();
                    this.Connection.Close();
                }
            }
        }

        /// <summary>
        /// 获取所有符合条件的字符串
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="key">键</param>
        /// <returns>符合条件的列表</returns>
        public List<String> getStringList(MySqlCondition condition,String key)
        {
            lock (this)
            {
                try
                {
                    this.Command = new MySqlCommand("SELECT " + key + " FROM " + this.Schema + "." + this.Table + " WHERE " + condition.condition + ";", this.Connection);
                    this.Connection.Open();
                    this.Reader = this.Command.ExecuteReader();

                    List<String> temp = new List<String>();

                    while (this.Reader.Read())
                    {
                        temp.Add(this.Reader.GetString(key));
                    }
                    return temp;
                }
                catch (MySqlException ex)
                {
                    ConsolePlusLib.Console.Out.println(Level.Severe, ex.ToString());
                    return null;
                }
                catch(SqlNullValueException)
                {
                    return null;
                }
                finally
                {
                    this.closeReader();
                    this.Connection.Close();
                }
            }
        }

        /// <summary>
        /// 获取所有符合条件的字符串
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="key">键</param>
        /// <returns>符合条件的列表</returns>
        public List<Int32> getIntegerList(MySqlCondition condition, String key)
        {
            lock (this)
            {
                try
                {
                    this.Command = new MySqlCommand("SELECT " + key + " FROM " + this.Schema + "." + this.Table + " WHERE " + condition.condition + ";", this.Connection);
                    this.Connection.Open();
                    this.Reader = this.Command.ExecuteReader();

                    List<Int32> temp = new List<Int32>();

                    while (this.Reader.Read())
                    {
                        temp.Add(this.Reader.GetInt32(key));
                    }

                    return temp;
                }
                catch (MySqlException ex)
                {
                    ConsolePlusLib.Console.Out.println(Level.Severe, ex.ToString());
                    return null;
                }
                finally
                {
                    this.closeReader();
                    this.Connection.Close();
                }
            }
        }

        /// <summary>
        /// 是否连接到MySql
        /// </summary>
        /// <returns>是否连接</returns>
        public Boolean isConnected()
        {
            lock (this)
            {
                try
                {
                    return this.Connection.State == ConnectionState.Connecting;
                }
                catch(Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 表格是否存在
        /// </summary>
        /// <returns></returns>
        public Boolean hasTable()
        {
            lock (this)
            {
                try
                {
                    this.Command = new MySqlCommand("SELECT * FROM " + this.Schema + "." + this.Table + ";", this.Connection);

                    this.Connection.Open();

                    this.Command.ExecuteNonQuery();

                    return true;
                }
                catch(MySqlException)
                {
                    return false;
                }
                finally
                {
                    this.Connection.Close();
                }
            }
        }


        /// <summary>
        /// 键或值是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public Boolean isExist(String key,String value)
        {
            lock (this)
            {
                try
                {
                    this.Command = new MySqlCommand("SELECT " + key + " FROM " + this.Schema + "." + this.Table + " WHERE " + key + " ='" + value + "';", this.Connection);
                    this.Connection.Open();
                    this.Reader = this.Command.ExecuteReader();

                    int i = 0;
                    while (this.Reader.Read())
                    {
                        i++;
                    }

                    return i > 0;
                }
                catch(MySqlException ex)
                {
                    ConsolePlusLib.Console.Out.println(Level.Severe, ex.ToString());
                    return false;
                }
                finally
                {
                    this.closeReader();
                    this.Connection.Close();
                }
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void close()
        {
            lock (this)
            {
                try
                {
                    if(this.Connection != null && this.Connection.State != ConnectionState.Closed)
                    {
                        this.Connection.Close();
                        this.closeReader();
                    }
                }
                catch(MySqlException ex)
                {
                    ConsolePlusLib.Console.Out.println(Level.Severe, ex.ToString());
                }
            }
        }

        /// <summary>
        /// 关闭Reader
        /// </summary>
        private void closeReader()
        {
            if(this.Reader != null && !this.Reader.IsClosed)
            {
                this.Reader.Close();
            }
        }
    }

}

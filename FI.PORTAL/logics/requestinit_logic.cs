using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using FI.PORTAL.dbconnect;
using FI.PORTAL.Models;


using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;
using System.Data.OleDb;
using System.Web.Hosting;
using System.Text;

namespace FI.PORTAL.logics
{
    public class requestinit_logic
    {
        SAP_SYSEntities SAP_SYS;
        FINANCE_MGR_SYSEntities FMG_SYS;

        customerModel cusModel;

        public List<orderModel> getcustomerOrderDetail(string cuscode)
        {
            try
            {
                orderModel order = new orderModel();
                List<orderModel> model = new List<orderModel>();

                using (SAP_SYS = new SAP_SYSEntities())
                {
                    var result = SAP_SYS.SalesOrderMasters.Where(a => a.CusCode == cuscode).Select(a => new
                    {
                        docid = a.DocID,
                        areaName = a.AreaName,
                        createdDate = a.CreatedDate,
                        createdBy = a.CreatedBy,
                        ordType = a.OrderType,
                        status = a.Status
                    }).ToList();
                    
                    foreach(var data in result)
                    {
                        order.Docid = (int) data.docid;
                        model.Add(order);
                    }
                }return null;
            }catch(Exception ex)
            {
                return null;
            }
        }

        public List<string> downloadreasons()
        {
            try
            {
                List<string> reasons = new List<string>();
                using (FMG_SYS = new FINANCE_MGR_SYSEntities())
                {
                    var result = FMG_SYS.ReasonMasters.Select(a => new { id = a.ReasonID, reason = a.Reason }).ToList();
                    if (result != null)
                    {
                        foreach (var item in result)
                        {
                            reasons.Add(item.id + "|" + item.reason);
                        }
                    }
                }
                return reasons;
            }catch(Exception ex) { return null; }
        }

        public List<string> downloadReqLevel(int curLevel)
        {
            try
            {
                List<string> level = new List<string>();
                using (FMG_SYS = new FINANCE_MGR_SYSEntities())
                {
                    if (curLevel == 1)
                    {
                        var result = FMG_SYS.CurrentStatusLvlMasters.Where(a => a.CurrStatusID < 5 && a.CurrStatusID > 1).Select(x => new { id = x.CurrStatusID, text = x.StatusText }).ToList();
                        if (result != null)
                        {
                            foreach (var item in result)
                            {
                                level.Add(item.id + "|" + item.text);
                            }
                        }
                    }
                    else
                    {
                        var result = FMG_SYS.CurrentStatusLvlMasters.Where(a => a.CurrStatusID < curLevel).Select(x => new { id = x.CurrStatusID, text = x.StatusText }).ToList();
                        if (result != null)
                        {
                            foreach (var item in result)
                            {
                                level.Add(item.id + "|" + item.text);
                            }
                        }
                    }
                }
                return level;
            }
            catch (Exception ex) { return null; }
        }

        public List<string> LoadHistory(int reqid)
        {
            try
            {
                List<string> history = new List<string>();
                using(FMG_SYS = new FINANCE_MGR_SYSEntities())
                {
                    var result = FMG_SYS.vw_Requests.Where(a => a.ReqID == reqid).Select(a => new { name = a.CusName, code = a.CusCode, limit = a.CreditLimit, area = a.CusSGDescription, exposure = a.CreditExposure.ToString(), action = a.RecentAction }).ToList();
                    foreach(var item in result)
                    {
                        history.Add(item.name);
                        history.Add(item.limit);
                        history.Add(item.area);
                        history.Add(item.exposure);
                        history.Add(item.action);
                        history.Add(item.code.ToString());
                    }
                    return history;
                }

            }catch(Exception ex)
            {
                return null;
            }
        }

        public customerModel getCustomerBasicDetail(string cuscode)
        {
            try
            {
                using (SAP_SYS = new SAP_SYSEntities())
                {
                    var result = SAP_SYS.CustomerMasters.Where(a => a.CusCode == cuscode).Join(SAP_SYS.CustomerOutstandings,
                        cus => cus.CusCode,
                        ccode => ccode.CusCode,
                        (cus, ccode) => new
                        {
                            id = cus.CusID,
                            name = cus.CusName,
                            salescode = cus.CusSalesGroup,
                            salesarea = cus.CusSGDescription,
                            limit = ccode.CreditLimit
                        }).FirstOrDefault();
                    if(result != null)
                    {
                        cusModel = new customerModel
                        {
                            Cusid = (int)result.id,
                            Cusname = result.name,
                            SalesCode = result.salescode,
                            SalesArea = result.salesarea,
                            Crdlimit = result.limit
                        };                      
                    }
                }
                return cusModel;
            }catch(Exception ex)
            {
                return null;
            }
        }

        public Int64 InitiateRequest(string cuscode, string filename, string salescode, string allowlimit, string comment, string user, string rtype,
            string crdExpo, string existing)
        {
            try
            {
                using(FMG_SYS = new FINANCE_MGR_SYSEntities())
                {
                    using(DbContextTransaction transaction = FMG_SYS.Database.BeginTransaction())
                    {
                        try
                        {
                            RequestHeader header = new RequestHeader(){
                                CusCode = Convert.ToDecimal(cuscode),
                                SalesCode = salescode,
                                AllowCreditLimit = Convert.ToDecimal(allowlimit), //Entitle Crd limit
                                InitialComment = comment,
                                CurrentStatusLevel = 2,
                                ReqStatus = "PENDING",
                                EnhancementStatus = 1,
                                CreatedDate = DateTime.Now,
                                CreatedBy = user,
                                RequestType = rtype,
                                RecentAction = "FORWARD",
                                CreditExposure = Convert.ToDecimal(crdExpo)
                            };

                            FMG_SYS.RequestHeaders.Add(header);
                            FMG_SYS.SaveChanges();
                            Int64 hid = (int) header.ReqID;

                            //Int64 did = AddRequestComment
                            //    (hid, allowlimit, comment, reason, user);

                            RequestDetail detail = new RequestDetail()
                            {
                                ReqID = hid,
                                CreatedBy = Convert.ToString(FMG_SYS.LoginDetails.Where(a => a.UserName == user).Select(a => new { role = a.Role }).FirstOrDefault().role),
                                CreatedByName = user,
                                Comment = comment,
                                CreditLimit = Convert.ToDecimal(0),
                                CreatedDate = DateTime.Now,
                                UserAction = "FORWARD",
                                ReasonID = 1
                            };

                            FMG_SYS.RequestDetails.Add(detail);
                            FMG_SYS.SaveChanges();
                            Int64 did = (Int64)detail.CommentID;
                            transaction.Commit();

                            return hid;

                        }catch(Exception ex)
                        {
                            transaction.Rollback();
                        }
                    }
                }
                return 0;
            }catch(Exception ex) { return 0; }
        }

        public void saveFile(Int64 reqid,string filename)
        {
            try
            {
                using(FMG_SYS = new FINANCE_MGR_SYSEntities())
                {
                    using (DbContextTransaction transaction = FMG_SYS.Database.BeginTransaction())
                    {
                        try
                        {
                            var result = FMG_SYS.RequestHeaders.Where(a => a.ReqID == reqid).SingleOrDefault();
                            if (result != null)
                            {
                                result.FileName = filename;
                                FMG_SYS.SaveChanges();
                                transaction.Commit();
                            }                                                       
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                        }
                    }
                }
            }
            catch { }

        }

        public Int64 AddRequestComment(Int64 hid, string allowlimit, string comment, string reason, string user, string action,int revertLevel)
        {
            try
            {
                using (FMG_SYS = new FINANCE_MGR_SYSEntities())
                {
                    using (DbContextTransaction transaction = FMG_SYS.Database.BeginTransaction())
                    {
                        try
                        {
                            RequestDetail detail = new RequestDetail()
                            {
                                ReqID = hid,
                                CreatedBy = Convert.ToString(FMG_SYS.LoginDetails.Where(a => a.UserName == user).Select(a => new { role = a.Role }).FirstOrDefault().role),
                                CreatedByName = user,
                                ReasonID = Convert.ToInt16(reason),
                                Comment = comment,
                                CreditLimit = Convert.ToDecimal(allowlimit),
                                CreatedDate = DateTime.Now,
                                UserAction = action
                            };

                            FMG_SYS.RequestDetails.Add(detail);
                            FMG_SYS.SaveChanges();
                            Int64 did = (Int64)detail.CommentID;


                            var result = FMG_SYS.RequestHeaders.Where(a => a.ReqID == hid).SingleOrDefault();
                            if(result != null)
                            {
                                if (action.Equals("FORWARD"))
                                {
                                    if (result.RecentAction.Equals("REVERSE"))
                                    {
                                        string lastLevel =
                                        FMG_SYS.RequestDetails.Where(a => a.ReqID == hid && a.UserAction.Equals("REVERSE")).OrderByDescending(a => a.CommentID).Take(1).SingleOrDefault().CreatedBy;
                                        result.CurrentStatusLevel = Convert.ToInt16(lastLevel);
                                    }
                                    else
                                    {
                                        result.CurrentStatusLevel = (FMG_SYS.RequestHeaders.Where(a => a.ReqID == hid).SingleOrDefault().CurrentStatusLevel) + 1;
                                    }
                                    result.RecentAction = "FORWARD";
                                    FMG_SYS.SaveChanges();
                                }else if (action.Equals("REVERSE"))
                                {
                                    result.ReqStatus = "PENDING";
                                    result.RecentAction = "REVERSE";
                                    result.CurrentStatusLevel = revertLevel;
                                    FMG_SYS.SaveChanges();
                                }else if (action.Equals("APPROVE"))
                                {
                                    result.ReqStatus = "PENDING";
                                    result.CurrentStatusLevel = 1;
                                    result.RecentAction = "APPROVE";
                                    FMG_SYS.SaveChanges();
                                }

                                else if (action.Equals("COMPLETED"))
                                {
                                    result.ReqStatus = "COMPLETED";
                                    result.RecentAction = "DONE";
                                    result.SAPupdateAmount = Convert.ToDecimal(allowlimit);
                                    FMG_SYS.SaveChanges();
                                }
                                else if (action.Equals("HOLD"))
                                {
                                    result.ReqStatus = "HOLD";
                                    result.RecentAction = "HOLD";
                                    result.SAPupdateAmount = Convert.ToDecimal(0);
                                    FMG_SYS.SaveChanges();
                                }
                                else if (action.Equals("NOT-REQUIRED"))
                                {
                                    result.ReqStatus = "NOT-REQUIRED";
                                    result.RecentAction = "STOP";
                                    result.SAPupdateAmount = Convert.ToDecimal(0);
                                    result.EnhancementStatus = 2;
                                    FMG_SYS.SaveChanges();
                                }
                            }

                            transaction.Commit();

                            return did;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                        }

                    }
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public List<requestModel> viewRequest(decimal cuscode_,int year_,int month_, int status_, int role, string user_)
        {
            try
            {
                List<requestModel> requests = new List<requestModel>();
                requestModel rmodel;
                SAP_SYS = new SAP_SYSEntities();
                FMG_SYS = new FINANCE_MGR_SYSEntities();

                //using(FMG_SYS = new FINANCE_MGR_SYSEntities())
                //{
                    
                    if (status_ == 1) //My Pendings
                    {
                        if(role == 1)// FOR CREDIT OFFICES >>>> MULTIPLE OFFICERS CREATE REQUESTS
                        {
                            var result = FMG_SYS.vw_Requests.
                                Where(a => a.CusCode.ToString().Contains(cuscode_.ToString())
                                    && a.year == year_
                                    && a.month == month_
                                    && a.CurrentStatusLevel == role
                                    && a.CreatedBy == user_
                                    && a.ReqStatus == "PENDING").
                            Select(x => new
                            {
                                reqId = x.ReqID,
                                cusName = x.CusName,
                                cusCode = x.CusCode,
                                CreditLimit = x.CreditLimit,
                                comment = x.InitialComment,
                                curStatus = x.StatusText,
                                curStatusID = x.CurrentStatusLevel,
                                count = x.size_,
                                reqStatus = x.ReqStatus,
                                date = x.CreatedDate,
                                filename = x.FileName,
                                rtype = x.RequestType,
                                scode = x.CusSGDescription,
                                saplimit = x.SAPupdateAmount
                            }).ToList();

                            foreach (var item in result)
                            {
                                rmodel = new requestModel()
                                {
                                    Reqid = item.reqId.ToString(),
                                    Cusname = item.cusName,
                                    Cuscode = item.cusCode.ToString(),
                                    Crdlimit = item.CreditLimit,
                                    Comments = item.comment,
                                    Curstatus = item.curStatus,
                                    StatusID = (int)item.curStatusID,
                                    Count_ = (int)item.count,
                                    Reqstatus = item.reqStatus,
                                    Date = item.date.ToString(),
                                    FileName = item.filename,
                                    RType = item.rtype,
                                    SaleCode = item.scode,
                                    SAPlimit = item.saplimit.ToString()
                                };
                                requests.Add(rmodel);
                            }
                    }
                    else
                    {
                        var result = FMG_SYS.vw_Requests.
                            Where(a => a.CusCode.ToString().Contains(cuscode_.ToString())
                                && a.year == year_
                                && a.month == month_
                                && a.CurrentStatusLevel == role
                                && a.ReqStatus == "PENDING").
                        Select(x => new
                        {
                            reqId = x.ReqID,
                            cusName = x.CusName,
                            cusCode = x.CusCode,
                            CreditLimit = x.CreditLimit,
                            comment = x.InitialComment,
                            curStatus = x.StatusText,
                            curStatusID = x.CurrentStatusLevel,
                            count = x.size_,
                            reqStatus = x.ReqStatus,
                            date = x.CreatedDate,
                            filename = x.FileName,
                            rtype = x.RequestType,
                            scode = x.CusSGDescription,
                            saplimit = x.SAPupdateAmount
                        }).ToList();

                        foreach (var item in result)
                        {
                            rmodel = new requestModel()
                            {
                                Reqid = item.reqId.ToString(),
                                Cusname = item.cusName,
                                Cuscode = item.cusCode.ToString(),
                                Crdlimit = item.CreditLimit,
                                Comments = item.comment,
                                Curstatus = item.curStatus,
                                StatusID = (int)item.curStatusID,
                                Count_ = (int)item.count,
                                Reqstatus = item.reqStatus,
                                Date = item.date.ToString(),
                                FileName = item.filename,
                                RType = item.rtype,
                                SaleCode = item.scode,
                                SAPlimit = item.saplimit.ToString()
                            };
                            requests.Add(rmodel);
                        }
                    }


                        

                        
                    }
                    else if(status_ == 2) //ALL REQ PENDING
                    {
                        var result = FMG_SYS.vw_Requests.Where(a => a.CusCode.ToString().Contains(cuscode_.ToString()) && a.year == year_ && a.month == month_ && a.ReqStatus == "PENDING" ).
                        Select(x => new
                        {
                            reqId = x.ReqID,
                            cusName = x.CusName,
                            cusCode = x.CusCode,
                            CreditLimit = x.CreditLimit,
                            comment = x.InitialComment,
                            curStatus = x.StatusText,
                            curStatusID = x.CurrentStatusLevel,
                            count = x.size_,
                            reqStatus = x.ReqStatus,
                            date = x.CreatedDate,
                            filename = x.FileName,
                            rtype = x.RequestType,
                            scode = x.CusSGDescription,
                            saplimit = x.SAPupdateAmount
                        }).ToList();

                        foreach (var item in result)
                        {
                            rmodel = new requestModel()
                            {
                                Reqid = item.reqId.ToString(),
                                Cusname = item.cusName,
                                Cuscode = item.cusCode.ToString(),
                                Crdlimit = item.CreditLimit,
                                Comments = item.comment,
                                Curstatus = item.curStatus,
                                StatusID = (int)item.curStatusID,
                                Count_ = (int)item.count,
                                Reqstatus = item.reqStatus,
                                Date = item.date.ToString(),
                                FileName = item.filename,
                                RType =item.rtype,
                                SaleCode = item.scode,
                                SAPlimit = item.saplimit.ToString()
                            };
                            requests.Add(rmodel);
                        }
                    }
                    else if (status_ == 3) //ALL REQ APPROVED
                    {
                        var result = FMG_SYS.vw_Requests.Where(a => a.CusCode.ToString().Contains(cuscode_.ToString()) && a.year == year_ && a.month == month_ && a.ReqStatus == "COMPLETED").
                        Select(x => new
                        {
                            reqId = x.ReqID,
                            cusName = x.CusName,
                            cusCode = x.CusCode,
                            CreditLimit = x.CreditLimit,
                            comment = x.InitialComment,
                            curStatus = x.StatusText,
                            curStatusID = x.CurrentStatusLevel,
                            count = x.size_,
                            reqStatus = x.ReqStatus,
                            date = x.CreatedDate,
                            filename = x.FileName,
                            rtype = x.RequestType,
                            scode = x.CusSGDescription,
                            saplimit = x.SAPupdateAmount
                        }).ToList();

                        foreach (var item in result)
                        {
                            rmodel = new requestModel()
                            {
                                Reqid = item.reqId.ToString(),
                                Cusname = item.cusName,
                                Cuscode = item.cusCode.ToString(),
                                Crdlimit = item.CreditLimit,
                                Comments = item.comment,
                                Curstatus = item.curStatus,
                                StatusID = (int)item.curStatusID,
                                Count_ = (int)item.count,
                                Reqstatus = item.reqStatus,
                                Date = item.date.ToString(),
                                FileName = item.filename,
                                RType =item.rtype,
                                SaleCode = item.scode,
                                SAPlimit = item.saplimit.ToString()
                            };
                            requests.Add(rmodel);
                        }
                    }
                else if (status_ == 4) //ALL REQ HOLD
                {
                    var result = FMG_SYS.vw_Requests.Where(a => a.CusCode.ToString().Contains(cuscode_.ToString()) && a.year == year_ && a.month == month_ && a.ReqStatus == "HOLD").
                    Select(x => new
                    {
                        reqId = x.ReqID,
                        cusName = x.CusName,
                        cusCode = x.CusCode,
                        CreditLimit = x.CreditLimit,
                        comment = x.InitialComment,
                        curStatus = x.StatusText,
                        curStatusID = x.CurrentStatusLevel,
                        count = x.size_,
                        reqStatus = x.ReqStatus,
                        date = x.CreatedDate,
                        filename = x.FileName,
                        rtype = x.RequestType,
                        scode = x.CusSGDescription,
                        saplimit = x.SAPupdateAmount
                    }).ToList();

                    foreach (var item in result)
                    {
                        rmodel = new requestModel()
                        {
                            Reqid = item.reqId.ToString(),
                            Cusname = item.cusName,
                            Cuscode = item.cusCode.ToString(),
                            Crdlimit = item.CreditLimit,
                            Comments = item.comment,
                            Curstatus = item.curStatus,
                            StatusID = (int)item.curStatusID,
                            Count_ = (int)item.count,
                            Reqstatus = item.reqStatus,
                            Date = item.date.ToString(),
                            FileName = item.filename,
                            RType = item.rtype,
                            SaleCode = item.scode,
                            SAPlimit = item.saplimit.ToString()
                        };
                        requests.Add(rmodel);
                    }
                }
                else if (status_ == 5) //ALL REQ NOT-REQUIRED
                {
                    var result = FMG_SYS.vw_Requests.Where(a => a.CusCode.ToString().Contains(cuscode_.ToString()) && a.year == year_ && a.month == month_ && a.ReqStatus == "NOT-REQUIRED").
                    Select(x => new
                    {
                        reqId = x.ReqID,
                        cusName = x.CusName,
                        cusCode = x.CusCode,
                        CreditLimit = x.CreditLimit,
                        comment = x.InitialComment,
                        curStatus = x.StatusText,
                        curStatusID = x.CurrentStatusLevel,
                        count = x.size_,
                        reqStatus = x.ReqStatus,
                        date = x.CreatedDate,
                        filename = x.FileName,
                        rtype = x.RequestType,
                        scode = x.CusSGDescription,
                        saplimit = x.SAPupdateAmount
                    }).ToList();

                    foreach (var item in result)
                    {
                        rmodel = new requestModel()
                        {
                            Reqid = item.reqId.ToString(),
                            Cusname = item.cusName,
                            Cuscode = item.cusCode.ToString(),
                            Crdlimit = item.CreditLimit,
                            Comments = item.comment,
                            Curstatus = item.curStatus,
                            StatusID = (int)item.curStatusID,
                            Count_ = (int)item.count,
                            Reqstatus = item.reqStatus,
                            Date = item.date.ToString(),
                            FileName = item.filename,
                            RType = item.rtype,
                            SaleCode = item.scode,
                            SAPlimit = item.saplimit.ToString()
                        };
                        requests.Add(rmodel);
                    }
                }


                //}
                return requests;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public List<commentModel> viewComments(Int64 reqID)
        {
            try
            {
                List<commentModel> comments = new List<commentModel>();
                commentModel model;
                using (FMG_SYS = new FINANCE_MGR_SYSEntities())
                {
                    var result = FMG_SYS.RequestDetails.Where(a => a.ReqID == reqID).Select(x => new
                    {
                        commentID = x.CommentID,
                        userName = x.CreatedByName,
                        role_ = FMG_SYS.CurrentStatusLvlMasters.Where(a => a.CurrStatusID.ToString() == x.CreatedBy).Select(a => new { role = a.StatusText }).FirstOrDefault().role,
                        createdDate = x.CreatedDate,
                        comment = x.Comment,
                        creditLimit = x.CreditLimit,
                        action = x.UserAction,
                        rtype = x.RequestHeader.RequestType
                    }).ToList().OrderByDescending(x=>x.commentID);

                    foreach(var item in result)
                    {
                        model = new commentModel()
                        {
                            CommID = (long) item.commentID,
                            Role = item.role_,
                            UserName = item.userName,
                            Comment = item.comment,
                            CreatedDate = item.createdDate.ToString(),
                            Limit = (decimal)item.creditLimit,
                            Action = item.action,
                            RType = item.rtype
                        };
                        comments.Add(model);
                    }
                }return comments;
            }catch(Exception ex)
            {
                return null;
            }
        }

        public int UserLogin(string uname, string pwd)
        {
            try
            {
               using(FMG_SYS = new FINANCE_MGR_SYSEntities())
               {
                    var result = FMG_SYS.LoginDetails.Where(a => a.UserName == uname && a.Password == pwd).Select(x => new { x.Role });
                    if(result != null)
                    {
                        int role = (int) result.FirstOrDefault().Role;
                        return role;
                    }
               }return 0;

            }catch(Exception ex) { return 0; }
        }



        /// <summary>
        /// FOR LOAD ENCRYPT IMAGES
        /// </summary>
        /// <returns></returns>
        public List<MemoryStream> Getimages(long commid)
        {
            try
            {
                List<MemoryStream> streams = new List<MemoryStream>();

                FMG_SYS = new FINANCE_MGR_SYSEntities();
                var img = FMG_SYS.RequestImages.Where(a => a.ReqID == commid).ToList();
                if (img != null)
                {
                    var imgbyte = img.FirstOrDefault().Image1;
                    streams.Add(GetMemory(imgbyte));

                    var imgbyte2 = img.FirstOrDefault().Image2;
                    streams.Add(GetMemory(imgbyte2));

                    var imgbyte3 = img.FirstOrDefault().Image3;
                    streams.Add(GetMemory(imgbyte3));
                }
                return streams;
            }
            catch { return null; }
        }


        private MemoryStream GetMemory(byte [] asci)
        {
            try
            {
                ASCIIEncoding ascii = new ASCIIEncoding();
                String decoded = ascii.GetString(asci);
                byte[] imgbyte = Convert.FromBase64String(decoded);
                MemoryStream MS = new MemoryStream(imgbyte, 0, imgbyte.Length);
                return MS;
            }
            catch { return null; }
        }
    }
}
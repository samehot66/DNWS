﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
namespace DNWS
{
    class TwitterApiPlugin : TwitterPlugin
    {
        private List<User> GetUser()
        {
            using (var context = new TweetContext())
            {
                try
                {
                    List<User> users = context.Users.Where(b => true).Include(b => b.Following).ToList();
                    return users;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        
        public override HTTPResponse GetResponse(HTTPRequest request)
        {
            HTTPResponse response = new HTTPResponse(200);
           
            string user = request.getRequestByKey("user");
            string password = request.getRequestByKey("password");
            string following = request.getRequestByKey("following");
            string fllw_timeline = request.getRequestByKey("timeline");
            string msg = request.getRequestByKey("msg");
            string []url =  request.Filename.Split("?");
            try
            {
                if (url[0] == "users")
                {
                    if (request.Method == "GET")
                    {
                        string js = JsonConvert.SerializeObject(GetUser());
                        response.body = Encoding.UTF8.GetBytes(js);
                    }
                    else if (request.Method == "POST")
                    {
                        if (user != null && password != null)
                        {
                            Twitter.AddUser(user, password);
                        }
                    }
                    else if (request.Method == "DELETE")
                    {
                        if (user != null)
                        {
                            Twitter.RemoveUser(user);
                        }
                    }
                }
                else if (url[0] == "following")
                {
                    if (request.Method == "GET")
                    {
                        Twitter tw = new Twitter(user);
                        string js = JsonConvert.SerializeObject(tw.GetFollowing());
                        response.body = Encoding.UTF8.GetBytes(js);
                    }
                    else if (request.Method == "POST")
                    {
                        if (user != null && following != null)
                        {
                            Twitter tw = new Twitter(user);
                            tw.AddFollowing(following);
                        }
                    }
                    else if (request.Method == "DELETE")
                    {
                        if (user != null && following != null)
                        {
                            Twitter tw = new Twitter(user);
                            tw.RemoveFollowing(following);

                        }
                    }
                }
                else if (url[0] == "tweets")
                {
                    if (user != null)
                    {
                        if (request.Method == "GET")
                        {
                            Twitter tw = new Twitter(user);
                            string js = JsonConvert.SerializeObject(tw.GetUserTimeline()); //userself timeline TO TEST using ?user=a
                            response.body = Encoding.UTF8.GetBytes(js);
                            if (fllw_timeline != null) //if following timeline not equal null that means select following timeline TO TEST using ?user=w&timeline=a
                            {
                                string js1 = JsonConvert.SerializeObject(tw.GetFollowingTimeline());
                                response.body = Encoding.UTF8.GetBytes(js1);
                            }
                        }
                        else if (request.Method == "POST")
                        {
                            Twitter tw = new Twitter(user);
                            tw.PostTweet(msg); 
                        }
                    }
                }
            }catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                Console.WriteLine(ex.ToString());
                sb.Append(String.Format("Error [{0}], please go back to <a href=\"/twitter\">login page</a> to try again", ex.Message));
                response.body = Encoding.UTF8.GetBytes(sb.ToString());
                return response;
            }
            response.type = "application/json; charset=UTF-8";
            return response;
        }
    
    }
}

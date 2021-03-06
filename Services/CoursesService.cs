﻿using System.Collections.Generic;
using System.Linq;
using A02.Entities;
using A02.Models;
using A02.Services.Exceptions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace A02.Services
{
    /// <summary>
    /// A service to connect to the database being used
    /// Takes care of all the needed queries to the database
    /// </summary>
    public class CoursesService : ICoursesService
    {
        private readonly AppDataContext _db;

        public CoursesService(AppDataContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Searches for a certain semester (current one if none given)
        /// and returns either a list of courses being taught that semester
        /// or throws an AppObjectNotFoundException if none are found.
        /// </summary>
        /// <param name="semester">An empty string or a certain semester</param>
        /// <returns>A list of CourseLiteDTO models</returns>
        /// <throws>AppObjectNotFoundException</throws>
        public List<CourseLiteDTO> GetCoursesBySemester(string semester)
        {
            if (string.IsNullOrEmpty(semester))
            {
                semester = "20163"; // Current semester hardcoded
            }

            var list = (from c in _db.Courses
                        join ct in _db.CourseTemplates on c.TemplateId equals ct.CourseId
                        where c.Semester == semester
                        select new CourseLiteDTO
                        {
                            Id = c.Id,
                            Name = ct.Name,
                            Semester = c.Semester,
                            StartDate = c.StartDate,
                            EndDate = c.EndDate
                        }).ToList();
            if(!list.Any())
            {
                throw new AppObjectNotFoundException();
            }
            return list;
        }

        /// <summary>
        /// Searches for a course with 'id' as it's Id and returns either a
        /// CourseLiteDTO object with that course's info or throws an
        /// AppObjectNotFoundException if no course with that 'id' is found
        /// </summary>
        /// <param name="id">An Id of a course</param>
        /// <returns>A CourseLiteDTO model</returns>
        /// <throws>AppObjectNotFoundException</throws>
        public CourseLiteDTO GetCourseById(int id)
        {
            var course = (from c in _db.Courses
                          join ct in _db.CourseTemplates on c.TemplateId equals ct.CourseId
                          where c.Id == id
                          select new CourseLiteDTO
                          {
                              Id = c.Id,
                              Name = ct.Name,
                              Semester = c.Semester,
                              StartDate = c.StartDate,
                              EndDate = c.EndDate
                          }).SingleOrDefault();
            if (course == null) throw new AppObjectNotFoundException();
            return course;
        }

        /// <summary>
        /// Updates the only mutable attributes (StartDate and EndDate) of
        /// a course with 'id' as it's Id. Throws an AppObjectNotFoundException
        /// if a course isn't found.
        /// </summary>
        /// <param name="id">The course's Id</param>
        /// <param name="sDate">The course's new starting date</param>
        /// <param name="eDate">The course's new end date</param>
        /// <throws>AppObjectNotFoundException</throws>
        public void UpdateCourseDates(int id, string sDate, string eDate)
        {
            var course = (from c in _db.Courses
                          where c.Id == id
                          select c).SingleOrDefault();

            if (course == null)
            {
                throw new AppObjectNotFoundException();
            }

            course.StartDate = sDate;
            course.EndDate = eDate;
            _db.SaveChanges();
        }

        /// <summary>
        /// Checks if any students are enrolled in a course and removes
        /// their connections before removing the course from the database.
        /// Throws an AppObjectNotFoundException if a course with 'id' as it's Id
        /// </summary>
        /// <param name="id">The course's Id</param>
        /// <throws>AppObjectNotFoundException</throws>
        public void DeleteCourse(int id)
        {
            var course = (from c in _db.Courses
                          where c.Id == id
                          select c).SingleOrDefault();
            if(course == null) throw new AppObjectNotFoundException();

            var connections = (from con in _db.StudentConnections
                              where con.CourseId == id
                              select con).DefaultIfEmpty();
            if (connections.Any())
            {
                foreach (var con in connections)
                {
                    _db.StudentConnections.Remove(con);
                }
            }
            _db.Courses.Remove(course);
            _db.SaveChanges();
        }

        /// <summary>
        /// Sends a list of StudentLiteDTO's to the api if any student's are
        /// enrolled in the course and if a course exists with 'id' as it's Id.
        /// If no course or student's are found, an AppObjectNotFoundException
        /// is thrown.
        /// </summary>
        /// <param name="id">The course's Id</param>
        /// <returns>A list of StudentLiteDTO models</returns>
        /// <throws>AppObjectNotFoundException</throws>
        public List<StudentLiteDTO> GetAllStudentsInCourse(int id)
        {
            var students = (from s in _db.Students
                            join sc in _db.StudentConnections on s.SSN equals sc.StudentId
                            where sc.CourseId == id
                            select new StudentLiteDTO
                            {
                                Name = s.Name,
                                SSN = s.SSN
                            }).ToList();
            if (!students.Any()) throw new AppObjectNotFoundException();
            return students;
        }

        /// <summary>
        /// Tries to add a student to a course. If either one or neither exist, or if
        /// a connection already exists, the database throws a DbUpdateException.
        /// That exception is used to figure out what went wrong and then an
        /// AppObjectNotFoundException or an AppObjectExistsException is thrown.
        /// 
        /// If no DbUpdateException is thrown, a connection has made between the course
        /// and the student and (s)he's now enrolled in that course.
        /// </summary>
        /// <param name="cId">The course's Id</param>
        /// <param name="sId">The student's SSN</param>
        /// <throws>AppObjectNotFoundException</throws>
        /// <throws>AppObjectExistsException</throws>
        public void AddStudentToCourse(int cId, string sId)
        {
            _db.StudentConnections.Add(new StudentConnection
            {
                CourseId = cId,
                StudentId = sId
            });

            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                var sqliteException = e.InnerException as SqliteException;
                // SqLite Error code 19 is for constraint violation
                if (sqliteException == null || sqliteException.SqliteErrorCode != 19) throw;
                if (sqliteException.Message.Trim().StartsWith("SQLite Error 19: \'FOREIGN KEY"))
                {
                    throw new AppObjectNotFoundException();
                }
                if (sqliteException.Message.Trim().StartsWith("SQLite Error 19: \'UNIQUE"))
                {
                    throw new AppObjectExistsException();
                }
                throw;
            }
        }

        /// <summary>
        /// If a student with 'sId' as SSN isn't connected to a course
        /// with 'cId' as its Id then an AppObjectNotFoundException is
        /// thrown. Else the connection is deleted and the student is
        /// taking that course anymore.
        /// </summary>
        /// <param name="cId">The course's Id</param>
        /// <param name="sId">The student's SSN</param>
        /// <throws>AppObjectNotFoundException</throws>
        public void RemoveStudentFromCourse(int cId, string sId)
        {
            var connection = (from con in _db.StudentConnections
                              where con.CourseId == cId && con.StudentId == sId
                              select con).SingleOrDefault();
            if(connection == null) throw new AppObjectNotFoundException();
            _db.StudentConnections.Remove(connection);
            _db.SaveChanges();
        }
    }
}

using DAL.Repository;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class UsersController(IRepository<User> userRepository) : Controller
{
    public IActionResult Index()
    {
        var users = userRepository.GetAll();
        return View(users);
    }

    public IActionResult Details(int id)
    {
        var user = userRepository.Get(id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Id,Name,Email")] User user)
    {
        if (ModelState.IsValid)
        {
            userRepository.Add(user);
            return RedirectToAction(nameof(Index));
        }
        return View(user);
    }

    public IActionResult Edit(int id)
    {
        var user = userRepository.Get(id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("Id,Name,Email")] User user)
    {
        if (id != user.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                userRepository.Update(user);
            }
            catch (Exception)
            {
                if (userRepository.Get(user.Id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(user);
    }

    public IActionResult Delete(int id)
    {
        var user = userRepository.Get(id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        userRepository.Delete(id);
        return RedirectToAction(nameof(Index));
    }
}

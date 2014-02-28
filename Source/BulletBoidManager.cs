﻿using BulletMLLib;
using FlockBuddy;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace BulletFlockBuddy
{
	public class BulletBoidManager : Flock, IBulletManager
	{
		#region Members

		private SimpleBulletManager BulletManager { get; set; }

		#endregion //Members

		#region Properties

		/// <summary>
		/// How fast time moves in this game.
		/// Can be used to do slowdown, speedup, etc.
		/// </summary>
		/// <value>The time speed.</value>
		public float TimeSpeed 
		{ 
			get
			{
				return BulletManager.TimeSpeed;
			}
			set
			{
				BulletManager.TimeSpeed = value;
			}
		}

		/// <summary>
		/// Change the size of this bulletml script
		/// If you want to reuse a script for a game but the size is wrong, this can be used to resize it
		/// </summary>
		/// <value>The scale.</value>
		public float Scale
		{ 
			get
			{
				return BulletManager.Scale;
			}
			set
			{
				BulletManager.Scale = value;
			}
		}

		#endregion //Properties

		public BulletBoidManager(PositionDelegate playerDelegate)
		{
			Debug.Assert(null != playerDelegate);
			BulletManager = new SimpleBulletManager(playerDelegate);
		}

		/// <summary>
		/// a mathod to get current position of the player
		/// This is used to target bullets at that position
		/// </summary>
		/// <returns>The position to aim the bullet at</returns>
		/// <param name="targettedBullet">the bullet we are getting a target for</param>
		public Vector2 PlayerPosition(Bullet targettedBullet)
		{
			return BulletManager.PlayerPosition(targettedBullet);
		}
		
		/// <summary>
		/// Create a new bullet.
		/// </summary>
		/// <returns>A shiny new bullet</returns>
		public Bullet CreateBullet()
		{
			//create the new bullet
			BulletBoid myBullet = new BulletBoid(this);

			BulletManager.InitBullet(myBullet);

			return myBullet;
		}
		
		public void RemoveBullet(Bullet deadBullet)
		{
			BulletManager.RemoveBullet(deadBullet);
		}

		public override void Update(GameTime curTime)
		{
			//the base class updates the flocking part of the dudes
			base.Update(curTime);

			//update the bullet part of the dude
			BulletManager.Update();
		}

		/// <summary>
		/// Clean up all the unused/dead bullets.
		/// </summary>
		private void FreeBullets()
		{
			for (int i = 0; i < BulletManager.Bullets.Count; i++)
			{
				BulletBoid bullet = BulletManager.Bullets[i] as BulletBoid;
				Debug.Assert(null != bullet);

				if (!bullet.Used)
				{
					//remove from the flock also
					RemoveBoid(bullet.MyBoid);

					//remove from the list of bullets
					BulletManager.Bullets.Remove(bullet);

					i--;
				}
			}
		}

		/// <summary>
		/// remove all the bullets from play
		/// </summary>
		public void Clear()
		{
			//clear out the flock
			Clear();

			//clear out the bullets
			BulletManager.Clear();
		}
	}
}

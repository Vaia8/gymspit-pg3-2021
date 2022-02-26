using System;
using System.IO;


namespace Lecture19Composition
{
	class Character
	{
		public const string TURN_CHOICE_ATTACK = "attack";
		public const string TURN_CHOICE_WAIT = "wait";
		public const string TURN_CHOICE_DEFEND = "defend";

		private Controller controller;

		private string name;

		private int hp;
		private int maxHp;

		private int attack;
		private int defense;
		private int defenseBonus;
		Random random = new Random();


		public string Name
		{
			get {
				return name;
			}
		}


		public int Hp
		{
			get {
				return hp;
			}
		}


		public int MaxHp
		{
			get {
				return maxHp;
			}
		}


		public bool Alive
		{
			get {
				return hp > 0;
			}
		}


		public int Attack
		{
			get {
				return attack;
			}
		}


		public int Defense
		{
			get {
				return defense;
			}
		}


		public Character(Controller controller, string name, int maxHp, int attack, int defense)
		{
			this.controller = controller;
			this.name = name;
			this.maxHp = maxHp;
			this.attack = attack;
			this.defense = defense;

			Reset();
		}


		public void TakeTurn(TextWriter output, Character enemy, Die die)
		{
			string action = controller.ChooseAction(this, enemy);

			switch (action) {
				case TURN_CHOICE_ATTACK:
					AttackEnemy(output, enemy, die);
					break;

				case TURN_CHOICE_WAIT:
					Wait(output, die);
					break;
				case TURN_CHOICE_DEFEND:
					Defend(output);
					break;

				default:
					output.WriteLine("{0} does nothing...", name);
					break;
			}
		}

		private void Defend(TextWriter output)
		{
			int oldDefenseBonus = defenseBonus;
			defenseBonus = defenseBonus + 2;
			output.WriteLine("Defense bonus increased from {0} to {1}.", oldDefenseBonus, defenseBonus);
		}
		public void Reset()
		{
			hp = maxHp;
			defenseBonus = 0;
		}


		private void AttackEnemy(TextWriter output, Character enemy, Die die)
		{
			output.WriteLine("{0} attacks {1}!", name, enemy.Name);
			int attackRoll = attack + die.Roll();
			enemy.ReceiveAttack(output, attackRoll, die);
		}


		private void ReceiveAttack(TextWriter output, int attackRoll, Die die)
		{
			int defenseRoll = defense + die.Roll() + defenseBonus;
			int damage = attackRoll - defenseRoll;

				int oldDefenseBonus = defenseBonus;
				defenseBonus = Math.Max(0, defenseBonus - 1);
			if (oldDefenseBonus != defenseBonus)
			{
				output.WriteLine("Defense bonus decreased from {0} to {1}.", oldDefenseBonus, defenseBonus);
			}
			if (damage > 0) {
				hp -= damage;
				output.WriteLine("{0} takes {1} damage!", name, damage);
			} else {
				output.WriteLine("{0} takes no damage!", name);
			}
		}


		private void Wait(TextWriter output, Die die)
		{
			output.WriteLine("{0} waits and rolls a die...", name);
			output.WriteLine("They rolled a {0}!", die.Roll());
			int healing = random.Next(2);
			if (hp + healing > maxHp)
			{
				hp = maxHp;
				output.WriteLine("{0}´s HP increased by {1}", name, maxHp-hp );
			}
            else 
			{ 
				hp = hp + healing;
				output.WriteLine("{0}´s HP increased by {1}", name, healing);
			}

		}
	}
}

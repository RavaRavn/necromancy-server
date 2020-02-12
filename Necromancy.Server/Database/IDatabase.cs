using System.Collections.Generic;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database
{
    public interface IDatabase
    {
        void Execute(string sql);

        /// <summary>
        /// Return true if database was created, or false if not.
        /// </summary>
        bool CreateDatabase();

        // Account
        Account CreateAccount(string name, string mail, string hash);
        Account SelectAccountById(int accountId);
        Account SelectAccountByName(string accountName);
        bool UpdateAccount(Account account);
        bool DeleteAccount(int accountId);

        // Soul
        bool InsertSoul(Soul soul);
        Soul SelectSoulById(int soulId);
        Soul SelectSoulByName(string soulName);
        List<Soul> SelectSoulsByAccountId(int accountId);
        bool UpdateSoul(Soul soul);
        bool DeleteSoul(int soulId);

        // ShortcutBar
        bool InsertShortcutBar(ShortcutBar shortcutBar);
        ShortcutBar SelectShortcutBarById(int shortcutBarId);
        bool UpdateShortcutBar(ShortcutBar shortcutBar);
        bool DeleteShortcutBar(int shortcutBarId);

        // SkillTreeItem
        bool InsertSkillTreeItem(SkillTreeItem skillTreeItem);
        SkillTreeItem SelectSkillTreeItemById(int id);
        List<SkillTreeItem> SelectSkillTreeItemsByCharId(int charId);
        SkillTreeItem SelectSkillTreeItemByCharSkillId(int charId, int skillId);
        bool UpdateSkillTreeItem(SkillTreeItem skillTreeItem);
        bool DeleteSkillTreeItem(int id);

        // Character
        bool InsertCharacter(Character character);
        Character SelectCharacterById(int characterId);
        Character SelectCharacterBySlot(int soulId, int slot);
        List<Character> SelectCharactersByAccountId(int accountId);
        List<Character> SelectCharactersBySoulId(int soulId);
        bool UpdateCharacter(Character character);
        bool DeleteCharacter(int characterId);
        List<Character> SelectCharacters();


        // NpcSpawn
        bool InsertNpcSpawn(NpcSpawn npcSpawn);
        List<NpcSpawn> SelectNpcSpawns();
        List<NpcSpawn> SelectNpcSpawnsByMapId(int mapId);
        bool UpdateNpcSpawn(NpcSpawn npcSpawn);
        bool DeleteNpcSpawn(int npcSpawnId);

        // Monster Spawn
        bool InsertMonsterSpawn(MonsterSpawn monsterSpawn);
        List<MonsterSpawn> SelectMonsterSpawns();
        List<MonsterSpawn> SelectMonsterSpawnsByMapId(int mapId);
        bool UpdateMonsterSpawn(MonsterSpawn monsterSpawn);
        bool DeleteMonsterSpawn(int monsterSpawnId);

        // Monster Coord
        bool InsertMonsterCoords(MonsterCoord monsterCoord);
        List<MonsterCoord> SelectMonsterCoords();
        List<MonsterCoord> SelectMonsterCoordsById(int Id);
        List<MonsterCoord> SelectMonsterCoordsByMonsterId(int monsterId);
        List<MonsterCoord> SelectMonsterCoordsByMapId(int mapId);
        bool UpdateMonsterCoord(MonsterCoord monsterCoord);
        bool DeleteMonsterCoord(int monsterSpawnId);

        // Items
        bool InsertItems(Items items);
        Items SelectitemsById(int itemsId);
        bool UpdateItems(Items items);
        bool DeleteItems(int itemsId);

        // Quest
        bool InsertQuest(Quest quest);
        Quest SelectQuestById(int questId);
        bool UpdateQuest(Quest quest);
        bool DeleteQuest(int questId);

        // Items
        bool InsertAuction(Auction Auction);
        Items SelectAuctionById(int AuctionId);
        bool UpdateAuction(Auction Auction);
        bool DeleteAuction(int AuctionId);
    }
}

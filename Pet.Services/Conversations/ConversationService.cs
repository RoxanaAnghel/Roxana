﻿using Pet.Database;
using Pet.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pet.Services.Conversations
{
    public class ConversationService : IConversationService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public ConversationService(UnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public Conversation[] GetAllForUser(Guid userId)
        {
            using (IUnitOfWork unitOfWork = unitOfWorkFactory.Create())
            {
                List<Conversation> conversations = new List<Conversation>();

                Database.Entities.Conversation[] dbConversations = unitOfWork.ConversationRepository.GetAllFor(userId);

                Guid[] otherUserIds = dbConversations.SelectMany(x => new Guid[2] { x.FromID, x.PetOwnerId }).Distinct().Where(x => x != userId).ToArray();
                Guid[] petIds = dbConversations.Select(x => x.PetID).ToArray();

                Database.Entities.UserDetails[] otherUsers = unitOfWork.UserDetailsRepository.GetByIds(otherUserIds);

                Database.Entities.Pet[] pets = unitOfWork.PetRepository.GetByIds(petIds);

                foreach (Database.Entities.Conversation dbConversation in dbConversations)
                {
                    Guid otherUserId = userId == dbConversation.FromID ? dbConversation.PetOwnerId : dbConversation.FromID;

                    Database.Entities.UserDetails otherUser = otherUsers.First(x => x.ID == otherUserId);
                    Database.Entities.Pet pet = pets.First(x => x.ID == dbConversation.PetID);

                    conversations.Add(new Conversation()
                    {
                        FromID = dbConversation.FromID,
                        FromUserImagineUrl = string.Empty, //sender.Image,
                        ID = dbConversation.ID,
                        PetID = dbConversation.PetID,
                        PetImagineUrl = pet.ImageUrl,
                        PetOwnerId = dbConversation.PetOwnerId,
                        PetOwnerImagineUrl = dbConversation.PetOwnerImagineUrl,
                        Status = dbConversation.Status
                    });
                }

                return conversations.ToArray();
            }
        }

        public Database.Entities.Conversation GetById(Guid id)
        {
            using (IUnitOfWork unitOfWork = unitOfWorkFactory.Create())
            {
                return unitOfWork.ConversationRepository.GetByID(id);
            }
        }

        public Database.Entities.Conversation GetConversationBeetwen(Guid userId, Guid petId)
        {
            using (IUnitOfWork unitOfWork = unitOfWorkFactory.Create())
            {
                if (!unitOfWork.ConversationRepository.ExistsBetween(userId, petId))
                {
                    Database.Entities.Pet pet = unitOfWork.PetRepository.GetByID(petId);
                    UserDetails petOwner = unitOfWork.UserDetailsRepository.GetByID(pet.OwnerID);
                    UserDetails user = unitOfWork.UserDetailsRepository.GetByID(userId);
                    Database.Entities.Conversation conversation = new Database.Entities.Conversation() { ID = Guid.NewGuid(), PetID = petId, PetOwnerId = pet.OwnerID, PetImagineUrl = pet.ImageUrl, FromID = userId };
                    unitOfWork.ConversationRepository.Create(conversation);
                    unitOfWork.Save();
                    return conversation;
                }
                return unitOfWork.ConversationRepository.GetConversationBetween(userId, petId);
            }
        }
    }
}
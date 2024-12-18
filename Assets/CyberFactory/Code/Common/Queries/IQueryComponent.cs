﻿using Scellecs.Morpeh;

namespace CyberFactory.Common.Queries {
    /// <summary>
    /// Query - это <b>Запрос</b>, который располагается на объекте и будет удален, после успешной обработки. <br/>
    /// <br/>
    /// Отличие от классического <b>Request</b> (see: <see cref="RequestBase"/>) в том,
    /// что для <b>Query</b> не требуется создавать отдельную сущность, 
    /// запрос может быть расположен на уже существующей. <br/>
    /// <br/>
    /// Так же <b>Request</b> может быть получен для обработки только один раз
    /// (с помощью <see cref="Request{TData}.Consume"/>), после чего он будет уничтожен.
    /// <b>Query</b> можно получить, посмотреть подходящие условия и если система не может обработать в данный момент,
    /// то пропустить данный запрос до следующего раза. <br/>
    /// <br/>
    /// Может быть в виде (но не только): <br/>
    /// - Call <br/>
    /// - Pending <br/>
    /// - Order <br/>
    /// </summary>
    public interface IQueryComponent : IComponent { }
}
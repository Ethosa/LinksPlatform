﻿using Platform.Helpers;

namespace Platform.Data.Core.Doublets
{
    public class UnaryNumberIncrementer<TLink> : LinksOperatorBase<TLink>, IIncrementer<TLink>
    {
        private readonly TLink _unaryOne;

        public UnaryNumberIncrementer(ILinks<TLink> links, TLink unaryOne)
            : base(links)
            => _unaryOne = unaryOne;

        public TLink Increment(TLink unaryNumber)
        {
            if (Equals(unaryNumber, _unaryOne))
                return Links.GetOrCreate(_unaryOne, _unaryOne);

            var source = Links.GetSource(unaryNumber);
            var target = Links.GetTarget(unaryNumber);

            if (Equals(source, target))
                return Links.GetOrCreate(unaryNumber, _unaryOne);
            else
                return Links.GetOrCreate(source, Increment(target));
        }
    }
}

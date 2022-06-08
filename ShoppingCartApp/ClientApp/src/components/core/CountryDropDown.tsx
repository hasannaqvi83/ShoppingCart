import { observer } from 'mobx-react';
import * as React from 'react';
import { useState } from 'react';
import { Dropdown, DropdownToggle, DropdownMenu, DropdownItem } from 'reactstrap';
import { useStoreContext } from '../../context/StoreContext';
import { Country } from '../../models/country';
export interface ICountryProps { }

export const CountryDropDown: React.FunctionComponent<ICountryProps> = observer((props: React.PropsWithChildren<ICountryProps>) => {
  const ctx = useStoreContext();
  // const [selectedCountryId, setSelectedCountryId] = useLocalStorage<number>('country', 1);
  const [isOpen, setIsOpen] = useState(false);

  const toggle = () => {
    setIsOpen(!isOpen);
  };
  const countrySelected = (country: Country) => (e: any) => {
    ctx.setSelectedCountryId(country.id); // e.currentTarget.textContent;
  };
  if (ctx.isCartReady) {
    return (
      <div className=" mb-3 mx-auto cart_item_wrapper text-right">
        <Dropdown isOpen={isOpen} toggle={toggle}>
          <DropdownToggle caret>
            {ctx.countries.filter(c => c.id === ctx.selectedCountryId)[0].name}
          </DropdownToggle>
          <DropdownMenu>
            {ctx.countries.map(country => (
              <DropdownItem onClick={countrySelected(country)} key={country.id}>
                {country.name}
              </DropdownItem>
            ))}
          </DropdownMenu>
        </Dropdown>
      </div>
    );
  } else return <></>;
});